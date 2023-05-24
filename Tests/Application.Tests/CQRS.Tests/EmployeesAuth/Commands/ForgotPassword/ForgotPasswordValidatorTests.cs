using Application.CQRS.EmployeesAuth.Commands.ForgotPassword;
using Domain.Interfaces.Validation;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.ForgotPassword;

public class ForgotPasswordValidatorTests
{
    private IValidator<ForgotPasswordCommand>? _validator;
    private readonly Mock<IEmployeeChecker> _mockEmployeeChecker;
    public ForgotPasswordValidatorTests()
    {
        _mockEmployeeChecker = new();
    }
    [Theory]
    [ClassData(typeof(ForgotPasswordCommandValidDataProvider))]
    public async Task ForgotPasswordValidator_ForValidForgotPasswordCommand_ShouldNotHaveAnyValidationErrors(ForgotPasswordCommand forgotPasswordCommand)
    {
        //Arrange 
        _mockEmployeeChecker
            .Setup(x => x.IsEmployeeExists(It.IsAny<string>()))
            .Returns(true);
        _validator = new ForgotPasswordValidator(_mockEmployeeChecker.Object);
        //Act
        var result = await _validator.TestValidateAsync(forgotPasswordCommand);
        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [ClassData(typeof(ForgotPasswordCommandInvalidDataProvider))]
    public async Task ForgotPasswordValidator_ForInvalidEmployeeEmail_ShouldHaveValidationErrorFor(ForgotPasswordCommand forgotPasswordCommand)
    {
        //Arrange
        _mockEmployeeChecker
            .Setup(x => x.IsEmployeeExists(It.IsAny<string>()))
            .Returns(true);
        _validator = new ForgotPasswordValidator(_mockEmployeeChecker.Object);
        //Act
        var result = await _validator.TestValidateAsync(forgotPasswordCommand);
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.ForgotPasswordDto.EmployeeEmail);
    }
    [Theory]
    [ClassData(typeof(ForgotPasswordCommandValidDataProvider))]
    public async Task ForgotPasswordValidator_ForNonExistingEmployee_ShouldHaveErrorForEmployeeEmail(ForgotPasswordCommand forgotPasswordCommand)
    {
        //Arrange
        _mockEmployeeChecker
            .Setup(x => x.IsEmployeeExists(It.IsAny<string>()))
            .Returns(false);
        _validator = new ForgotPasswordValidator(_mockEmployeeChecker.Object);
        //Act
        var result = await _validator.TestValidateAsync(forgotPasswordCommand);
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.ForgotPasswordDto.EmployeeEmail);
    }

}