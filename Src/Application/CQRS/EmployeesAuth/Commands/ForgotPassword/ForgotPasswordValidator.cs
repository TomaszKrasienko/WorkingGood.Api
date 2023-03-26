using Domain.Interfaces.Validation;
using FluentValidation;

namespace Application.CQRS.EmployeesAuth.Commands.ForgotPassword;

public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordValidator(IEmployeeChecker employeeChecker)
    {
        RuleFor(x => x.ForgotPasswordDto.EmployeeEmail)
            .NotNull()
            .NotEmpty()
            .EmailAddress()
            .Must(x => employeeChecker.IsEmployeeExists(x))
            .WithMessage("Employee does not exists");
    }
}