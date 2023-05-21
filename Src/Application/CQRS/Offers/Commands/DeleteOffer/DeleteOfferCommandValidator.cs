using Domain.Interfaces.Validation;
using FluentValidation;

namespace Application.CQRS.Offers.Commands.DeleteOffer;

public class DeleteOfferCommandValidator : AbstractValidator<DeleteOfferCommand>
{
    public DeleteOfferCommandValidator(IOfferChecker offerChecker)
    {
        RuleFor(x => x.OfferId)
            .NotNull()
            .NotEqual(default(Guid))
            .Must(x => offerChecker.IsOfferExists(x) == true)
            .WithMessage("Offer does not exists");
    }
}