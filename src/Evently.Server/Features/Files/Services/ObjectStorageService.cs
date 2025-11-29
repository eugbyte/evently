using Azure;
using Azure.AI.ContentSafety;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Evently.Server.Common.Extensions;
using Evently.Server.Domains.Interfaces;
using Evently.Server.Domains.Models;
using Microsoft.Extensions.Options;

namespace Evently.Server.Features.Files.Services;

// Based on https://tinyurl.com/5pam66xn
public sealed class ObjectStorageService : IObjectStorageService
{
	private readonly BlobServiceClient _blobServiceClient;
	private readonly ContentSafetyClient? _contentSafetyClient;
	private readonly ILogger<ObjectStorageService> _logger;

	public ObjectStorageService(IOptions<Settings> settings, ILogger<ObjectStorageService> logger)
	{
		_logger = logger;
		_blobServiceClient = new BlobServiceClient(
			settings.Value.StorageAccount.AzureStorageConnectionString
		);

		try
		{
			_contentSafetyClient = new ContentSafetyClient(
				endpoint: new Uri(settings.Value.AzureAiFoundry.ContentSafetyEndpoint),
				credential: new AzureKeyCredential(settings.Value.AzureAiFoundry.ContentSafetyKey)
			);
		}
		catch (Exception ex)
		{
			// silence the error
			_logger.LogError(
				"error creating content safety client: {message}. Content moderation skipped.",
				ex.Message
			);
		}
	}

	public async Task<Uri> UploadFile(
		string containerName,
		string fileName,
		BinaryData binaryData,
		string mimeType = "application/octet-stream"
	)
	{
		BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(
			containerName
		);
		await containerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);

		BlobClient blobClient = containerClient.GetBlobClient(fileName);

		BlobUploadOptions uploadOptions = new()
		{
			HttpHeaders = new BlobHttpHeaders { ContentType = mimeType },
			// https://tinyurl.com/ms4hvsta
			// By default, there will be condition to prevent overwrite.
			// Set the conditions to null so that overwrite will be allowed.
			Conditions = null,
		};
		await blobClient.UploadAsync(binaryData, uploadOptions);
		return blobClient.Uri;
	}

	public async Task<bool> IsFileExists(string containerName, string fileName)
	{
		BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(
			containerName
		);
		BlobClient blobClient = containerClient.GetBlobClient(fileName);
		Response<bool> result = await blobClient.ExistsAsync();
		return result.Value;
	}

	public Task<Uri> GetFileUri(string containerName, string fileName)
	{
		BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(
			containerName
		);
		BlobClient blobClient = containerClient.GetBlobClient(fileName);
		return Task.FromResult(blobClient.Uri);
	}

	public async Task<BinaryData> GetFile(string containerName, string fileName)
	{
		BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(
			containerName
		);
		BlobClient blobClient = containerClient.GetBlobClient(fileName);

		Response<bool> result = await blobClient.ExistsAsync();
		if (!result.Value)
		{
			throw new FileNotFoundException($"File {fileName} not found");
		}

		using MemoryStream ms = new();
		try
		{
			await blobClient.DownloadToAsync(ms);
		}
		catch (Exception ex)
		{
			_logger.LogError("error getting file: {}", ex.Message);
		}

		byte[] bytes = ms.ToArray();
		BinaryData data = BinaryData.FromBytes(bytes);
		return data;
	}

	public async Task<bool> PassesContentModeration(BinaryData binaryData)
	{
		if (_contentSafetyClient is null)
		{
			return true;
		}

		ContentSafetyImageData image = new(binaryData);
		AnalyzeImageOptions request = new(image);
		Response<AnalyzeImageResult> response;
		try
		{
			response = await _contentSafetyClient.AnalyzeImageAsync(request);
		}
		catch (RequestFailedException ex)
		{
			_logger.LogContentModerationError(ex.Status.ToString(), ex.ErrorCode ?? "", ex.Message);
			throw;
		}

		AnalyzeImageResult result = response.Value;
		int dangerScore = result
			.CategoriesAnalysis.Select(v => v.Severity ?? 0)
			.DefaultIfEmpty(0)
			.Aggregate((a, b) => a + b);
		return dangerScore == 0;
	}
}
