using Evently.Server.Domains.Entities;
using FluentValidation;

namespace Evently.Server.Features.Gatherings.Services;

public sealed class GatheringValidator : AbstractValidator<Gathering> {
	public GatheringValidator() {
		RuleFor((exhibition) => exhibition.Name).NotEmpty().WithMessage("Name is required.");
		RuleFor((exhibition) => exhibition.Description).NotEmpty().WithMessage("Description is required.");
		RuleFor((exhibition) => exhibition.Start).NotEmpty().WithMessage("Starting Date is required.");
		RuleFor((exhibition) => exhibition.End).NotEmpty().WithMessage("End Date is required.");
		RuleFor((exhibition) => exhibition.OrganiserId).NotEmpty().WithMessage("Event Organiser Id is required.");
		RuleForEach((exhibition) => exhibition.Bookings).Custom((value, context) => {
			if (value.GatheringId == 0) {
				context.AddFailure("ExhibitionId is required.");
			}

			if (string.IsNullOrEmpty(value.BookingId)) {
				context.AddFailure("BookingEventId is required.");
			}
		});
	}
}