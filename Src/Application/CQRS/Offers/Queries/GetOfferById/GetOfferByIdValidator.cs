using FluentValidation;

namespace Application.CQRS.Offers.Queries.GetOfferById;

public class GetOfferByIdValidator : AbstractValidator<GetOfferByIdCommand>
{
    public GetOfferByIdValidator()
    {
        RuleFor(x => x.OfferId)
            .NotNull()
            .NotEmpty()
            .NotEqual(default(Guid));
    }
}