namespace Evently.Server.Common.Extensions;

public static class UtilsExtension
{
	public static Uri RootUri(this HttpRequest request)
	{
		UriBuilder uriBuilder = new() { Scheme = request.Scheme, Host = request.Host.Host };
		if (request.Host.Port.HasValue)
		{
			uriBuilder.Port = request.Host.Port.Value;
		}

		return uriBuilder.Uri;
	}

	public static async Task<BinaryData> ToBinaryData(this IFormFile file)
	{
		using MemoryStream ms = new();
		await file.CopyToAsync(ms);
		byte[] bytes = ms.ToArray();
		return BinaryData.FromBytes(bytes);
	}
}
