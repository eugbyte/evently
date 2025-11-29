using Evently.Server.Domains.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.ComponentModel.DataAnnotations;

namespace Evently.Server.Features.Files.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FilesController(ILogger<FilesController> logger, IObjectStorageService objectStorageService)
	: ControllerBase {
	[HttpGet("object-storage/{bucket}", Name = "GetFile")]
	public async Task<ActionResult> GetFile(string bucket, [Required] [FromQuery] string fileName) {
		logger.LogInformation("fileName: {}", fileName);
		try {
			BinaryData binaryData = await objectStorageService.GetFile(bucket, fileName);
			logger.LogInformation("binaryData.MediaType: {}", binaryData.MediaType);
			string contentType = binaryData.MediaType ?? GetContentType(fileName);
			return File(fileContents: binaryData.ToArray(), fileDownloadName: fileName, contentType: contentType);
		} catch (Exception ex) {
			return ex switch {
				FileNotFoundException => NotFound($"File '{fileName}' not found in bucket '{bucket}'."),
				_ => StatusCode(statusCode: 500, "An unexpected error occurred while retrieving the file."),
			};
		}
	}

	private static string GetContentType(string fileName) {
		FileExtensionContentTypeProvider provider = new();
		if (!provider.TryGetContentType(fileName, contentType: out string? contentType)) {
			contentType = "application/octet-stream"; // Default fallback
		}

		return contentType;
	}
}