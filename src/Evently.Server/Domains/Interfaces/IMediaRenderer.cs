using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace Evently.Server.Domains.Interfaces;

public interface IMediaRenderer {
	Task<string> RenderComponentHtml<T>(Dictionary<string, object?> dictionary) where T : IComponent;
	BinaryData RenderQr(string qrData);

	[SuppressMessage("ReSharper",
		"UnusedMember.Global",
		Justification = "May need to convert ticket HTML to PDF in future")]
	BinaryData RenderPdf(string html);
}