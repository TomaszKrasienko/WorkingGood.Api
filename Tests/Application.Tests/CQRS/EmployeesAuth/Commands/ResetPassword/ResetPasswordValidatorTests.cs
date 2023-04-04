using Domain.Interfaces.Validation;
using FluentValidation;
using Moq;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.ResetPassword;

public class ResetPasswordValidatorTests
{
    private readonly Mock<IEmployeeChecker> _mockEmployeeChecker;
    private IValidator<ResetPasswordCommand>? _validator;

    public ResetPasswordValidatorTests()
    {
        _mockEmployeeChecker = new();
    }

    [Fact]
    public async Task ResetPasswordValidator_ForValidResetPasswordCommand_ShouldHaveNoErrors()
    {
        
    }
}