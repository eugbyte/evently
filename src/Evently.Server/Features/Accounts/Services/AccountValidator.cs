using Evently.Server.Domains.Entities;
using FluentValidation;

namespace Evently.Server.Features.Accounts.Services;

public sealed class AccountValidator : AbstractValidator<Account> {
	public AccountValidator() {
		RuleFor((member) => member.Name).NotEmpty().WithMessage("Name is required.");
		RuleFor((member) => member.Email).NotEmpty().WithMessage("Email is required.");
		RuleForEach((member) => member.Bookings).Custom((value, context) => {
			if (value.AttendeeId == string.Empty) {
				context.AddFailure("MemberId is required.");
			}

			if (string.IsNullOrEmpty(value.BookingId)) {
				context.AddFailure("BookingId is required.");
			}
		});
	}
}