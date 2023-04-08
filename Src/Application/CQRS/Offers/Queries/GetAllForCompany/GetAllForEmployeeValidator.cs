using FluentValidation;

namespace Application.CQRS.Offers.Queries.GetAllForCompany;

public class GetAllForEmployeeValidator : AbstractValidator<GetAllForCompanyQuery>
{
    public GetAllForEmployeeValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotNull()
            .NotEmpty();
    }
}