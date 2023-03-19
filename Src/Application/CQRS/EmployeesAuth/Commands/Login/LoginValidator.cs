using Domain.Interfaces.Validation;
using FluentValidation;

namespace Application.CQRS.EmployeesAuth.Commands.Login;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator(IEmployeeChecker employeeChecker)
    {
        RuleFor(x => x.CredentialsDto.Email)
            .NotEmpty()
            .NotNull()
            .Must(x => employeeChecker.IsEmployeeExists(x))
            .WithMessage("Wrong credentials");
        RuleFor(x => x.CredentialsDto.Password)
            .NotEmpty()
            .NotNull();
    }
}