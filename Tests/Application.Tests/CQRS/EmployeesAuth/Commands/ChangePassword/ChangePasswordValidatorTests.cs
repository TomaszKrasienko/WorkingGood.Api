using Application.CQRS.EmployeesAuth.Commands.ChangePassword;
using FluentValidation;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.ChangePassword;

public class ChangePasswordValidatorTests
{
    private IValidator<ChangePasswordCommand> _validator;
}