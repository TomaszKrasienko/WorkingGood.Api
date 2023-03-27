using Domain.Interfaces.Validation;
using FluentValidation;

namespace Application.CQRS.EmployeesAuth.Commands.VerifyEmployee;

public class VerifyEmployeeValidator : AbstractValidator<VerifyEmployeeCommand>
{
    public VerifyEmployeeValidator(IEmployeeChecker employeeChecker)
    {
        RuleFor(x => x.VerificationToken)
            .NotNull()
            .NotEmpty();
        When(x => x.VerificationToken != null, () =>
        {
            RuleFor(x => x.VerificationToken)
                .Must(x => employeeChecker.IsVerificationTokenExists(x))
                .WithMessage("Token is not exists");
        });
    }
}