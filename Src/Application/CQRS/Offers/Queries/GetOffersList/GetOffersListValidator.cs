using FluentValidation;

namespace Application.CQRS.Offers.Queries.GetOffersList;

public class GetOffersListValidator: AbstractValidator<GetOffersListQuery>
{
    public GetOffersListValidator()
    {
        RuleFor(x => x.GetOffersListRequestDto)
            .NotNull();
        When(x => x.GetOffersListRequestDto.CompanyOffers == true, () =>
        {
            RuleFor(x => x.EmployeeId)
                .NotNull()
                .NotEqual(default(Guid));
        });
    }
}