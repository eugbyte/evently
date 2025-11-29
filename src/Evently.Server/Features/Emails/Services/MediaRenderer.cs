using Evently.Server.Domains.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.HtmlRendering;
using PdfSharp;
using PdfSharp.Pdf;
using QRCoder;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using BlazorHtmlRenderer = Microsoft.AspNetCore.Components.Web.HtmlRenderer;

namespace Evently.Server.Features.Emails.Services;

public sealed class MediaRenderer(BlazorHtmlRenderer htmlRenderer) : IMediaRenderer {
	public async Task<string> RenderComponentHtml<T>(Dictionary<string, object?> dictionary) where T : IComponent {
		string html = await htmlRenderer.Dispatcher.InvokeAsync(async () => {
			ParameterView parameters = ParameterView.FromDictionary(dictionary);
			HtmlRootComponent output = await htmlRenderer.RenderComponentAsync<T>(parameters);
			return output.ToHtmlString();
		});
		return html;
	}

	public BinaryData RenderQr(string qrData) {
		using QRCodeGenerator qrGenerator = new();
		QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q);
		PngByteQRCode qrCode = new(qrCodeData);
		byte[] imageBytes = qrCode.GetGraphic(20);
		return BinaryData.FromBytes(imageBytes);
	}

	public BinaryData RenderPdf(string html) {
		using MemoryStream ms = new();
		using PdfDocument pdf = PdfGenerator.GeneratePdf(html, PageSize.A4);
		pdf.Save(ms);
		byte[] bytes = ms.ToArray();
		return BinaryData.FromBytes(bytes);
	}
}