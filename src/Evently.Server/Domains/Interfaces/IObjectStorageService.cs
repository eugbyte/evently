using System.Diagnostics.CodeAnalysis;

namespace Evently.Server.Domains.Interfaces;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public interface IObjectStorageService
{
	// mimeType determines the browser's behaviour when the file is accessed directly via the browser,
	// e.g. download ("application/octet-stream") or view ("images/*)" the file.
	Task<Uri> UploadFile(
		string containerName,
		string fileName,
		BinaryData binaryData,
		string mimeType = "application/octet-stream"
	);

	Task<Uri> GetFileUri(string containerName, string fileName);
	Task<BinaryData> GetFile(string containerName, string fileName);
	Task<bool> IsFileExists(string containerName, string fileName);
	Task<bool> PassesContentModeration(BinaryData binaryData);
}
