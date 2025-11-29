using System.Text.Json;
using Evently.Server.Common.Data;
using Evently.Server.Common.Extensions;
using Evently.Server.Domains.Entities;
using Evently.Server.Domains.Interfaces;
using Evently.Server.Domains.Models;
using Evently.Server.Features.Emails.Views;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NanoidDotNet;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Evently.Server.Features.Bookings.Services;

public sealed class BookingService(
	IMediaRenderer mediaRenderer,
	IObjectStorageService objectStorageService,
	IValidator<Booking> validator,
	IOptions<Settings> settings,
	AppDbContext db
) : IBookingService
{
	private readonly string _containerName = settings.Value.StorageAccount.AccountName;

	public async Task<Booking?> GetBooking(string bookingId)
	{
		return await db
			.Bookings.Include((b) => b.Account)
			.Include((b) => b.Gathering)
				.ThenInclude((g) => g!.GatheringCategoryDetails)
					.ThenInclude((detail) => detail.Category)
			.FirstOrDefaultAsync((be) => be.BookingId == bookingId);
	}

	public async Task<PageResult<Booking>> GetBookings(
		string? accountId,
		long? gatheringId,
		DateTimeOffset? checkInStart,
		DateTimeOffset? checkInEnd,
		DateTimeOffset? gatheringStartBefore,
		DateTimeOffset? gatheringStartAfter,
		DateTimeOffset? gatheringEndBefore,
		DateTimeOffset? gatheringEndAfter,
		bool? isCancelled,
		int? offset,
		int? limit
	)
	{
		IQueryable<Booking> query = db
			.Bookings.Where((b) => accountId == null || b.AttendeeId == accountId)
			.Where((b) => gatheringId == null || b.GatheringId == gatheringId)
			.Where((c) => checkInStart == null || checkInStart <= c.CheckInDateTime)
			.Where((b) => checkInEnd == null || b.CheckInDateTime <= checkInEnd)
			.Where((b) => isCancelled == null || b.CancellationDateTime.HasValue == isCancelled)
			.Where(
				(b) =>
					gatheringStartBefore == null
					|| b.Gathering != null && b.Gathering.Start <= gatheringStartBefore
			)
			.Where(
				(b) =>
					gatheringStartAfter == null
					|| b.Gathering != null && b.Gathering.Start >= gatheringStartAfter
			)
			.Where(
				(b) =>
					gatheringEndBefore == null
					|| b.Gathering != null && b.Gathering.End <= gatheringEndBefore
			)
			.Where(
				(b) =>
					gatheringEndAfter == null
					|| b.Gathering != null && b.Gathering.End >= gatheringEndAfter
			)
			.Include((b) => b.Account)
			.Include((b) => b.Gathering)
				.ThenInclude((g) => g!.GatheringCategoryDetails)
					.ThenInclude((detail) => detail.Category);

		int totalCount = await query.CountAsync();

		List<Booking> bookingEvents = await query
			.OrderByDescending((be) => be.CreationDateTime)
			.Skip(offset ?? 0)
			.Take(limit ?? int.MaxValue)
			.ToListAsync();

		return new PageResult<Booking> { Items = bookingEvents, TotalCount = totalCount };
	}

	public async Task<Booking> CreateBooking(BookingReqDto bookingReqDto)
	{
		Booking booking = bookingReqDto.ToBooking();
		ValidationResult validationResult = await validator.ValidateAsync(booking);
		if (!validationResult.IsValid)
		{
			throw new ArgumentException(
				$"Account has already booked this gathering (GatheringId: {booking.GatheringId})"
			);
		}

		booking.BookingId = $"book_{await Nanoid.GenerateAsync(size: 10)}";
		await db.Bookings.AddAsync(booking);
		await db.SaveChangesAsync();
		return (await GetBooking(booking.BookingId))!;
	}

	public async Task<Booking> UpdateBooking(string bookingId, BookingReqDto bookingReqDto)
	{
		Booking booking = bookingReqDto.ToBooking();

		ValidationResult validationResult = await validator.ValidateAsync(booking);
		if (!validationResult.IsValid)
		{
			throw new ArgumentException(
				string.Join("\n", values: validationResult.Errors.Select(e => e.ErrorMessage))
			);
		}

		Booking current =
			await db.Bookings.AsTracking().FirstOrDefaultAsync((be) => be.BookingId == bookingId)
			?? throw new KeyNotFoundException($"{booking.BookingId} not found");

		current.AttendeeId = booking.AttendeeId;
		current.GatheringId = booking.GatheringId;
		current.CreationDateTime = booking.CreationDateTime;
		current.CheckInDateTime = booking.CheckInDateTime;
		current.CheckoutDateTime = booking.CheckoutDateTime;
		current.CancellationDateTime = booking.CancellationDateTime;

		await db.SaveChangesAsync();
		return (await GetBooking(booking.BookingId))!;
	}

	public async Task<string> RenderTicket(string bookingId)
	{
		Booking? booking = await GetBooking(bookingId);
		if (booking?.Account is null || booking.Gathering is null)
		{
			throw new KeyNotFoundException(
				$"Booking with id: {bookingId} not found or related member or gathering is null"
			);
		}

		string qrData = JsonSerializer.Serialize(new { bookingEventId = bookingId });
		BinaryData binaryData = mediaRenderer.RenderQr(qrData);
		string fileName = $"bookings/{bookingId}/qrcode.png";

		Uri uri;
		bool isFileExists = await objectStorageService.IsFileExists(_containerName, fileName);
		if (!isFileExists)
		{
			uri = await objectStorageService.UploadFile(
				_containerName,
				fileName,
				binaryData,
				"image/png"
			);
		}
		else
		{
			uri = await objectStorageService.GetFileUri(_containerName, fileName);
		}

		Dictionary<string, object?> props = new()
		{
			{ "Booking", booking },
			{ "QrCodeUrl", uri.AbsoluteUri },
		};

		return await mediaRenderer.RenderComponentHtml<Ticket>(props);
	}
}
