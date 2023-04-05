using Application.CQRS.EmployeesAuth.Commands.ResetPassword;
using Domain.Interfaces.Validation;
using FluentValidation;
using FluentValidation.TestHelper;
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
        //Arrange
        ResetPasswordCommand resetPasswordCommand = new()
        {
            ResetPasswordDto = new()
            {
                NewPassword = "Test123!",
                ConfirmNewPassword = "Test123!",
                ResetToken = "Test"
            }
        };
        _mockEmployeeChecker.Setup(x => x.IsResetTokenExists(It.IsAny<string>()))
            .Returns(true);
        _validator = new ResetPasswordValidator(_mockEmployeeChecker.Object);
        //Act
        var result = await _validator.TestValidateAsync(resetPasswordCommand);
        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    [Fact]
    public async Task ResetPasswordValidator_ForNonExistingResetToken_ShouldHaveResetTokenError()
    {
        //Arrange
        ResetPasswordCommand resetPasswordCommand = new()
        {
            ResetPasswordDto = new()
            {
                NewPassword = "Test123!",
                ConfirmNewPassword = "Test123!",
                ResetToken = "Test"
            }
        };
        _mockEmployeeChecker.Setup(x => x.IsResetTokenExists(It.IsAny<string>()))
            .Returns(false);
        _validator = new ResetPasswordValidator(_mockEmployeeChecker.Object);
        //Act
        var result = await _validator.TestValidateAsync(resetPasswordCommand);
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.ResetPasswordDto.ResetToken);
    }
    [Theory]
    [ClassData(typeof(ResetPasswordTestsDataProvider))]
    public async Task ResetPasswordValidator_ForInvalidResetTokenCommand_ShouldHaveResetTokenError(ResetPasswordCommand resetPasswordCommand)
    {
        //Arrange
        _mockEmployeeChecker.Setup(x => x.IsResetTokenExists(It.IsAny<string>()))
            .Returns(false);
        _validator = new ResetPasswordValidator(_mockEmployeeChecker.Object);
        //Act
        var result = await _validator.TestValidateAsync(resetPasswordCommand);
        //Assert
        result.ShouldHaveAnyValidationError();
    }

}