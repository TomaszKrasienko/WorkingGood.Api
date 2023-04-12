using Application.CQRS.Offers.Queries.GetOfferStatus;
using Domain.Interfaces.Validation;
using FluentValidation;

namespace Application.CQRS.Offers.Queries.GetStatus;

public class GetStatusValidator : AbstractValidator<GetStatusQuery>
{
    public GetStatusValidator(IOfferChecker offerChecker)
    {
        RuleFor(x => x.OfferId)
            .NotNull()
            .NotEmpty();
        When(x => x.OfferId != null, () =>
        {
            RuleFor(x => x.OfferId)
                .Must(x => offerChecker.IsOfferExists(x))
                .WithMessage("Offer does not exists");
        });
    }
}