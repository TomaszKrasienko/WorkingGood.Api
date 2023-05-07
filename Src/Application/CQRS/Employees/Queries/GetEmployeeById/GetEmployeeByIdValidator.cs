using FluentValidation;

namespace Application.CQRS.Employees.Queries;

public class GetEmployeeByIdValidator : AbstractValidator<GetEmployeeByIdQuery>
{
    public GetEmployeeByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty()
            .NotEqual(default(Guid));
    }
}