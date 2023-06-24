using FluentValidation;

namespace Application.CQRS.Companies.Queries.GetCompanyByOfferId;

public class GetCompanyByOfferIdQueryValidator :  AbstractValidator<GetCompanyByOfferIdQuery>
{
    public GetCompanyByOfferIdQueryValidator()
    {
        RuleFor(x => x.OfferId)
            .NotNull()
            .NotEmpty()
            .NotEqual(default(Guid));
    }
}