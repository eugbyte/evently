using Evently.Server.Common.Extensions;
using Evently.Server.Domains.Entities;
using Evently.Server.Domains.Models;

namespace Evently.Server.Test.Common.Extensions;

public class MapperExtensionTests {
	[Fact]
	public void TestMapToGathering() {
		// Arrange (mock values)
		DateTimeOffset start = DateTimeOffset.UtcNow.AddDays(3);
		DateTimeOffset end = start.AddHours(2);

		GatheringReqDto dto = new(
			GatheringId: 0,
			"Mock Gathering",
			"Mock Description",
			start,
			end,
			CancellationDateTime: null,
			"Mock Location",
			"organizer-mock",
			"mock-cover.jpg",
			GatheringCategoryDetails: []
		);

		// Act
		Gathering entity = dto.ToGathering();

		// Assert
		Assert.NotNull(entity);
		Assert.Equal(dto.Name, entity.Name);
		Assert.Equal(dto.Description, entity.Description);
		Assert.Equal(dto.Start, entity.Start);
		Assert.Equal(dto.End, entity.End);
		Assert.Equal(dto.CancellationDateTime, entity.CancellationDateTime);
		Assert.Equal(dto.Location, entity.Location);
		Assert.Equal(dto.OrganiserId, entity.OrganiserId);
		Assert.Equal(dto.CoverSrc, entity.CoverSrc);
		Assert.NotNull(entity.GatheringCategoryDetails);
		Assert.Empty(entity.GatheringCategoryDetails);
	}


	[Fact]
	public void TestMapToBooking() {
		// Arrange (mock values)
		const string attendeeId = "attendee-mock";
		const string bookingId = "book_mock";
		const long gatheringId = 42L;

		DateTimeOffset creation = DateTimeOffset.UtcNow.AddDays(1);
		DateTimeOffset checkIn = creation.AddHours(1);
		DateTimeOffset checkout = creation.AddHours(2);
		DateTimeOffset cancellation = creation.AddHours(3);

		// Create DTO with mock values
		BookingReqDto dto = new(attendeeId, bookingId, gatheringId, creation, checkIn, checkout, cancellation);

		// Act
		Booking booking = dto.ToBooking();

		// Assert: direct field comparisons (no reflection)
		Assert.NotNull(booking);
		Assert.Equal(dto.AttendeeId, booking.AttendeeId);
		Assert.Equal(dto.GatheringId, booking.GatheringId);
		Assert.Equal(dto.CreationDateTime, booking.CreationDateTime);
		Assert.Equal(dto.CheckInDateTime, booking.CheckInDateTime);
		Assert.Equal(dto.CheckoutDateTime, booking.CheckoutDateTime);
		Assert.Equal(dto.CancellationDateTime, booking.CancellationDateTime);

		// If BookingId is part of the mapping, also validate it:
		// Assert.Equal(dto.BookingId, booking.BookingId);
	}

	[Fact]
	public void TestMapToBookingDto() {
		// Arrange (mock values)
		const string attendeeId = "attendee-mock";
		const long gatheringId = 7L;

		DateTimeOffset creation = DateTimeOffset.UtcNow.AddDays(2);
		DateTimeOffset checkIn = creation.AddHours(1);
		DateTimeOffset checkout = creation.AddHours(2);
		DateTimeOffset? cancellation = null;

		Booking booking = new() {
			AttendeeId = attendeeId,
			GatheringId = gatheringId,
			CreationDateTime = creation,
			CheckInDateTime = checkIn,
			CheckoutDateTime = checkout,
			CancellationDateTime = cancellation,
		};

		// Act
		BookingReqDto dto = booking.ToBookingDto();

		// Assert: direct field comparisons (no reflection)
		Assert.NotNull(dto);
		Assert.Equal(booking.AttendeeId, dto.AttendeeId);
		Assert.Equal(booking.GatheringId, dto.GatheringId);
		Assert.Equal(booking.CreationDateTime, dto.CreationDateTime);
		Assert.Equal(booking.CheckInDateTime, dto.CheckInDateTime);
		Assert.Equal(booking.CheckoutDateTime, dto.CheckoutDateTime);
		Assert.Equal(booking.CancellationDateTime, dto.CancellationDateTime);
	}


	[Fact]
	public void TestMapToAccountDto() {
		// Arrange (mock values)
		Account account = new() {
			Id = "acc_mock",
			Email = "mock@example.com",
		};

		// Act
		AccountDto accountDto = account.ToAccountDto();

		// Assert: direct field comparisons (no reflection)
		Assert.NotNull(accountDto);
		Assert.Equal(account.Id, accountDto.Id);
		Assert.Equal(account.Email, accountDto.Email);
	}
}