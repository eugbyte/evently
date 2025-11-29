using Evently.Server.Common.Extensions;
using Evently.Server.Domains.Entities;
using Evently.Server.Domains.Interfaces;
using Evently.Server.Domains.Models;
using Evently.Server.Features.Accounts.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Globalization;

namespace Evently.Server.Features.Gatherings.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class GatheringsController(
	IOptions<Settings> settings,
	ILogger<GatheringsController> logger,
	IGatheringService gatheringService,
	IObjectStorageService objectStorageService) : ControllerBase {
	private readonly string _containerName = settings.Value.StorageAccount.AccountName;

	[HttpGet("{gatheringId:long}", Name = "GetGathering")]
	public async Task<ActionResult<Gathering>> GetGathering(long gatheringId) {
		Gathering? customer = await gatheringService.GetGathering(gatheringId);
		if (customer is null) {
			return NotFound();
		}

		return Ok(customer);
	}

	[HttpGet("", Name = "GetGatherings")]
	public async Task<ActionResult<List<Gathering>>> GetGatherings(string? attendeeId,
		string? organiserId,
		string? name,
		DateTimeOffset? startDateBefore, DateTimeOffset? startDateAfter, DateTimeOffset? endDateBefore,
		DateTimeOffset? endDateAfter,
		bool? isCancelled,
		[FromQuery(Name = "categoryIds[]")] long[]? categoryIds,
		int? offset, int? limit) {
		logger.LogInformation("categoryIds: {}", string.Join(",", values: categoryIds ?? []));
		PageResult<Gathering> result = await gatheringService.GetGatherings(attendeeId,
			organiserId,
			name,
			startDateBefore,
			startDateAfter,
			endDateBefore,
			endDateAfter,
			isCancelled,
			categoryIds: categoryIds?.ToHashSet(),
			offset,
			limit);
		List<Gathering> exhibitions = result.Items;
		int total = result.TotalCount;
		HttpContext.Response.Headers.Append("Access-Control-Expose-Headers", "X-Total-Count");
		HttpContext.Response.Headers.Append("X-Total-Count", value: total.ToString(CultureInfo.InvariantCulture));
		return Ok(exhibitions);
	}

	[HttpPost("", Name = "CreateGathering")]
	public async Task<ActionResult<Gathering>> CreateGathering([FromForm] GatheringReqDto gatheringReqDto,
		[FromForm] IFormFile? coverImg) {
		gatheringReqDto = gatheringReqDto with { GatheringId = 0L };

		AuthenticateResult authenticationResult =
			await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
		if (!authenticationResult.Succeeded) {
			return Unauthorized();
		}

		if (coverImg != null) {
			string uri = await UploadCoverImage(gatheringReqDto.GatheringId, coverImg);
			gatheringReqDto = gatheringReqDto with { CoverSrc = uri };
		}

		Gathering gathering = await gatheringService.CreateGathering(gatheringReqDto);
		return Ok(gathering);
	}

	[HttpPut("{gatheringId:long}", Name = "UpdateGathering")]
	public async Task<ActionResult> UpdateGathering(long gatheringId, [FromForm] GatheringReqDto gatheringReqDto,
		[FromForm] IFormFile? coverImg) {
		Gathering? gathering = await gatheringService.GetGathering(gatheringId);
		if (gathering is null) {
			return NotFound();
		}

		if (!await this.IsResourceOwner(gathering.OrganiserId)) {
			return Forbid();
		}

		if (coverImg != null) {
			string uri = await UploadCoverImage(gatheringReqDto.GatheringId, coverImg);
			gatheringReqDto = gatheringReqDto with { CoverSrc = uri };
		}

		gathering = await gatheringService.UpdateGathering(gatheringId, gatheringReqDto);
		return Ok(gathering);
	}

	[HttpDelete("{gatheringId:long}", Name = "DeleteGathering")]
	public async Task<ActionResult<Gathering>> DeleteGathering(long gatheringId) {
		Gathering? exhibition = await gatheringService.GetGathering(gatheringId);
		if (exhibition is null) {
			return NotFound();
		}

		if (!await this.IsResourceOwner(exhibition.OrganiserId)) {
			return Forbid();
		}

		await gatheringService.DeleteGathering(gatheringId);
		return NoContent();
	}

	private async Task<string> UploadCoverImage(long gatheringId, IFormFile coverImg) {
		string fileName = $"gatherings/{gatheringId}/cover-image{Path.GetExtension(coverImg.FileName)}";
		BinaryData binaryData = await coverImg.ToBinaryData();
		bool isContentSafe = await objectStorageService.PassesContentModeration(binaryData);
		if (!isContentSafe) {
			return string.Empty;
		}

		Uri uri = await objectStorageService.UploadFile(_containerName,
			fileName,
			binaryData,
			mimeType: MimeTypes.GetMimeType(coverImg.FileName));
		return uri.AbsoluteUri;
	}
}