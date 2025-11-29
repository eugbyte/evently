using Evently.Server.Domains.Entities;
using FluentValidation;

namespace Evently.Server.Features.Bookings.Services;

public sealed class BookingValidator : AbstractValidator<Booking> {
	public BookingValidator() {
		RuleFor((booking) => booking.GatheringId).NotEmpty().WithMessage("GatheringId is required.");
		RuleFor((booking) => booking.AttendeeId).NotEmpty().WithMessage("AccountId is required.");
	}
}