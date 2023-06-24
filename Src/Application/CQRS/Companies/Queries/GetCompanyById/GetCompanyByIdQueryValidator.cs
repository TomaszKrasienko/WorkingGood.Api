using Application.CQRS.Employees.Queries;
using FluentValidation;

namespace Application.CQRS.Companies.Queries.GetCompanyById;

public class GetCompanyByIdQueryValidator : AbstractValidator<GetCompanyByIdQuery>
{
    public GetCompanyByIdQueryValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotNull()
            .NotEmpty()
            .NotEqual(default(Guid));
    }
}