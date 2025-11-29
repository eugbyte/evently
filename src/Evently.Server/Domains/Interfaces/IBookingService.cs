using Evently.Server.Domains.Entities;
using Evently.Server.Domains.Models;

namespace Evently.Server.Domains.Interfaces;

public interface IBookingService {
	Task<Booking?> GetBooking(string bookingId);

	Task<PageResult<Booking>> GetBookings(string? accountId, long? gatheringId,
		DateTimeOffset? checkInStart, DateTimeOffset? checkInEnd,
		DateTimeOffset? gatheringStartBefore, DateTimeOffset? gatheringStartAfter, DateTimeOffset? gatheringEndBefore,
		DateTimeOffset? gatheringEndAfter,
		bool? isCancelled, int? offset, int? limit);

	Task<Booking> CreateBooking(BookingReqDto bookingReqDto);
	Task<Booking> UpdateBooking(string bookingId, BookingReqDto bookingReqDto);
	Task<string> RenderTicket(string bookingId);
}