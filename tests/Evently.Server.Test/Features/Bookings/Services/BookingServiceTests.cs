using Evently.Server.Common.Adapters.Data;
using Evently.Server.Common.Data;
using Evently.Server.Domains.Entities;
using Evently.Server.Domains.Interfaces;
using Evently.Server.Domains.Models;
using Evently.Server.Features.Bookings.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace Evently.Server.Test.Features.Bookings.Services;

public class BookingServiceTests : IDisposable {
	private readonly IBookingService _bookingService;
	private readonly SqliteConnection _conn;
	private readonly AppDbContext _dbContext;

	public BookingServiceTests() {
		_conn = new SqliteConnection("Filename=:memory:");
		_conn.Open();

		// These options will be used by the context instances in this test suite, including the connection opened above.
		DbContextOptions<AppDbContext> contextOptions = new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlite(_conn)
			.Options;

		// Create the schema and seed some data
		AppDbContext dbContext = new(contextOptions);

		dbContext.Database.EnsureCreated();
		_dbContext = dbContext;

		Mock<IMediaRenderer> mediaRendererMock = new();
		Mock<IObjectStorageService> fileStorageServiceMock = new();
		IOptions<Settings> options = Options.Create(new Settings());

		_bookingService = new BookingService(mediaRendererMock.Object,
			fileStorageServiceMock.Object,
			validator: new BookingValidator(),
			options,
			_dbContext);
	}

	public void Dispose() {
		_dbContext.Dispose();
		_conn.Dispose();
	}

	[Fact]
	public async Task CreateBooking_WithValidData_ShouldCreateBooking() {
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
		Booking result = await _bookingService.CreateBooking(bookingReqDto);

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
		await Assert.ThrowsAsync<ArgumentException>(() => _bookingService.CreateBooking(invalidBookingReqDto));
	}

	[Fact]
	public async Task GetBooking_WithValidBookingId_ShouldReturnBooking() {
		// Act
		Booking? result = await _bookingService.GetBooking("book_abc123456");

		// Assert
		Assert.NotNull(result);
		Assert.Equal("book_abc123456", result.BookingId);
	}

	[Fact]
	public async Task UpdateBooking_WithNonExistentBookingId_ShouldThrowKeyNotFoundException() {
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
			_bookingService.UpdateBooking(nonExistentBookingId, updateRequest));
	}

	[Fact]
	public async Task UpdateBooking_WithCancellation_ShouldUpdateCancellationDateTime() {
		// Arrange
		DateTimeOffset cancellationTime = DateTimeOffset.Now.AddMinutes(30);
		Booking? booking = await _bookingService.GetBooking("book_abc123456");
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
		booking = await _bookingService.UpdateBooking("book_abc123456", updateRequest);

		// Assert
		Assert.NotNull(booking);
		Assert.Equal(cancellationTime, booking.CancellationDateTime);
	}
}