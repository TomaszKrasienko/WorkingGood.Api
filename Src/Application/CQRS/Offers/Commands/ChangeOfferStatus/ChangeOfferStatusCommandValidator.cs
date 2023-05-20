using Domain.Interfaces.Validation;
using FluentValidation;

namespace Application.CQRS.Offers.Commands.ChangeOfferStatus;

public class ChangeOfferStatusCommandValidator : AbstractValidator<ChangeOfferStatusCommand>
{
    public ChangeOfferStatusCommandValidator(IOfferChecker offerChecker)
    {
        RuleFor(x => x.OfferId)
            .NotNull()
            .NotEqual(default(Guid))
            .Must(x => offerChecker.IsOfferExists(x) == true)
            .WithMessage("Offer does not exist");
    }
}