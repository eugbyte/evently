using Evently.Server.Common.Data;
using Evently.Server.Common.Extensions;
using Evently.Server.Domains.Entities;
using Evently.Server.Domains.Interfaces;
using Evently.Server.Domains.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Evently.Server.Features.Gatherings.Services;

public sealed class GatheringService(AppDbContext db, IValidator<Gathering> validator)
	: IGatheringService
{
	public async Task<Gathering?> GetGathering(long gatheringId)
	{
		return await db
			.Gatherings.Include(gathering => gathering.Bookings)
			.Include(gathering => gathering.GatheringCategoryDetails)
				.ThenInclude(detail => detail.Category)
			.FirstOrDefaultAsync((gathering) => gathering.GatheringId == gatheringId);
	}

	public async Task<PageResult<Gathering>> GetGatherings(
		string? attendeeId,
		string? organiserId,
		string? name,
		DateTimeOffset? startDateBefore,
		DateTimeOffset? startDateAfter,
		DateTimeOffset? endDateBefore,
		DateTimeOffset? endDateAfter,
		bool? isCancelled,
		HashSet<long>? categoryIds,
		int? offset,
		int? limit
	)
	{
		IQueryable<Gathering> query = db
			.Gatherings.Where(
				(gathering) => name == null || EF.Functions.Like(gathering.Name, $"%{name}%")
			)
			.Where((gathering) => startDateBefore == null || gathering.Start <= startDateBefore)
			.Where((gathering) => startDateAfter == null || gathering.Start >= startDateAfter)
			.Where((gathering) => endDateBefore == null || gathering.End <= endDateBefore)
			.Where((gathering) => endDateAfter == null || gathering.End >= endDateAfter)
			.Where((gathering) => organiserId == null || gathering.OrganiserId == organiserId)
			.Where(gathering =>
				isCancelled == null || gathering.CancellationDateTime.HasValue == isCancelled
			)
			.Where(
				(gathering) =>
					categoryIds == null
					|| categoryIds.Count == 0
					|| gathering.GatheringCategoryDetails.Any(detail =>
						categoryIds.Contains(detail.CategoryId)
					)
			)
			.Where(
				(gathering) =>
					attendeeId == null
					|| gathering.Bookings.Any((be) => be.AttendeeId == attendeeId)
			)
			.Include(gathering => gathering.Bookings.Where((be) => be.AttendeeId == attendeeId))
			.Include(gathering => gathering.GatheringCategoryDetails)
				.ThenInclude(detail => detail.Category);

		int totalCount = await query.CountAsync();

		List<Gathering> gatherings = await query
			.OrderBy(gathering => gathering.Start)
			.Skip(offset ?? 0)
			.Take(limit ?? int.MaxValue)
			.Select((gathering) => gathering)
			.ToListAsync();

		return new PageResult<Gathering> { Items = gatherings, TotalCount = totalCount };
	}

	public async Task<Gathering> CreateGathering(GatheringReqDto gatheringReqDto)
	{
		Gathering gathering = gatheringReqDto.ToGathering();
		ValidationResult validationResult = await validator.ValidateAsync(gathering);
		if (!validationResult.IsValid)
		{
			throw new ArgumentException(
				string.Join(", ", values: validationResult.Errors.Select(e => e.ErrorMessage))
			);
		}

		db.Gatherings.Add(gathering);
		await db.SaveChangesAsync();
		return gathering;
	}

	public async Task<Gathering> UpdateGathering(long gatheringId, GatheringReqDto gatheringReqDto)
	{
		Gathering gathering = gatheringReqDto.ToGathering();
		ValidationResult validationResult = await validator.ValidateAsync(gathering);
		if (!validationResult.IsValid)
		{
			throw new ArgumentException(
				string.Join(", ", values: validationResult.Errors.Select(e => e.ErrorMessage))
			);
		}

		Gathering current =
			await db
				.Gatherings.AsTracking()
				.Include((g) => g.GatheringCategoryDetails)
				.FirstOrDefaultAsync((ex) => ex.GatheringId == gatheringId)
			?? throw new KeyNotFoundException($"{gatheringId} not found");

		current.Name = gathering.Name;
		current.Description = gathering.Description;
		current.Start = gathering.Start;
		current.End = gathering.End;
		current.Location = gathering.Location;
		current.CoverSrc = gathering.CoverSrc;
		current.GatheringCategoryDetails = gathering.GatheringCategoryDetails;

		await db.SaveChangesAsync();
		return current;
	}

	public async Task DeleteGathering(long gatheringId)
	{
		Gathering gathering = await db
			.Gatherings.AsTracking()
			.SingleAsync((gathering) => gathering.GatheringId == gatheringId);
		db.Remove(gathering);
		await db.SaveChangesAsync();
	}
}
