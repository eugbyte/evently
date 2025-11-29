using Evently.Server.Common.Data;
using Evently.Server.Domains.Entities;
using Evently.Server.Domains.Interfaces;
using Evently.Server.Domains.Models;
using Evently.Server.Features.Bookings.Services;
using Evently.Server.Test.Common.Setup;
using Microsoft.Extensions.Options;
using Moq;

namespace Evently.Server.Test.Features.Bookings.Services;

public class BookingServiceTests(DatabaseFixture dbFixture) : IClassFixture<DatabaseFixture> {
	private readonly Mock<IObjectStorageService> _fileStorageServiceMock = new();

	private readonly Mock<IMediaRenderer> _mediaRendererMock = new();
	private readonly IOptions<Settings> _options = Options.Create(new Settings());

	[Fact]
	public async Task CreateBooking_WithValidData_ShouldCreateBooking() {
		AppDbContext dbContext = await dbFixture.GetDbContext();
		BookingService bookingService = new(
			_mediaRendererMock.Object,
			_fileStorageServiceMock.Object,
			validator: new BookingValidator(),
			_options,
			dbContext
		);

		DateTimeOffset now = DateTimeOffset.Now;
		// Arrange
		BookingReqDto bookingReqDto = new(
			"book_abc",
			GatheringId: 1,
			AttendeeId: "empty-user-12345",
			CancellationDateTime: null,
			CheckInDateTime: null,
			CheckoutDateTime: null,
			CreationDateTime: now
		);

		// Act
		Booking result = await bookingService.CreateBooking(bookingReqDto);

		// Assert
		Assert.NotNull(result);
		Assert.NotNull(result.BookingId);
		Assert.Equal(bookingReqDto.GatheringId, result.GatheringId);
		Assert.Equal(bookingReqDto.AttendeeId, result.AttendeeId);
		Assert.Equal(bookingReqDto.CancellationDateTime, result.CancellationDateTime);
		Assert.Equal(bookingReqDto.CheckInDateTime, result.CheckInDateTime);
		Assert.Equal(bookingReqDto.CheckoutDateTime, result.CheckoutDateTime);
		Assert.Equal(bookingReqDto.CreationDateTime, result.CreationDateTime);
	}

	[Fact]
	public async Task CreateBooking_WithEmptyAttendeeId_ShouldThrowException() {
		AppDbContext dbContext = await dbFixture.GetDbContext();
		BookingService bookingService = new(
			_mediaRendererMock.Object,
			_fileStorageServiceMock.Object,
			validator: new BookingValidator(),
			_options,
			dbContext
		);

		DateTimeOffset now = DateTimeOffset.Now;
		// Arrange
		BookingReqDto invalidBookingReqDto = new(
			"book_abc",
			GatheringId: 1,
			AttendeeId: "",
			CancellationDateTime: null,
			CheckInDateTime: null,
			CheckoutDateTime: null,
			CreationDateTime: now
		);

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentException>(() =>
			bookingService.CreateBooking(invalidBookingReqDto)
		);
	}

	[Fact]
	public async Task GetBooking_WithValidBookingId_ShouldReturnBooking() {
		AppDbContext dbContext = await dbFixture.GetDbContext();
		BookingService bookingService = new(
			_mediaRendererMock.Object,
			_fileStorageServiceMock.Object,
			validator: new BookingValidator(),
			_options,
			dbContext
		);

		// Act
		Booking? result = await bookingService.GetBooking("book_abc123456");

		// Assert
		Assert.NotNull(result);
		Assert.Equal("book_abc123456", result.BookingId);
	}

	[Fact]
	public async Task UpdateBooking_WithNonExistentBookingId_ShouldThrowKeyNotFoundException() {
		AppDbContext dbContext = await dbFixture.GetDbContext();
		BookingService bookingService = new(
			_mediaRendererMock.Object,
			_fileStorageServiceMock.Object,
			validator: new BookingValidator(),
			_options,
			dbContext
		);

		// Arrange
		const string nonExistentBookingId = "book_nonexistent";
		BookingReqDto updateRequest = new(
			nonExistentBookingId,
			GatheringId: 1,
			AttendeeId: "user_test",
			CancellationDateTime: null,
			CheckInDateTime: null,
			CheckoutDateTime: null,
			CreationDateTime: DateTimeOffset.Now
		);

		// Act & Assert
		await Assert.ThrowsAsync<KeyNotFoundException>(() =>
			bookingService.UpdateBooking(nonExistentBookingId, updateRequest)
		);
	}

	[Fact]
	public async Task UpdateBooking_WithCancellation_ShouldUpdateCancellationDateTime() {
		AppDbContext dbContext = await dbFixture.GetDbContext();
		BookingService bookingService = new(
			_mediaRendererMock.Object,
			_fileStorageServiceMock.Object,
			validator: new BookingValidator(),
			_options,
			dbContext
		);

		// Arrange
		DateTimeOffset cancellationTime = DateTimeOffset.Now.AddMinutes(30);
		Booking? booking = await bookingService.GetBooking("book_abc123456");
		Assert.NotNull(booking);

		BookingReqDto updateRequest = new(
			booking.BookingId,
			GatheringId: booking.GatheringId,
			AttendeeId: booking.AttendeeId,
			CancellationDateTime: cancellationTime,
			CheckInDateTime: null,
			CheckoutDateTime: null,
			CreationDateTime: booking.CreationDateTime
		);

		// Act
		booking = await bookingService.UpdateBooking("book_abc123456", updateRequest);

		// Assert
		Assert.NotNull(booking);
		Assert.Equal(cancellationTime, booking.CancellationDateTime);
	}
}
