using Application.CQRS.EmployeesAuth.Commands.VerifyEmployee;
using Domain.Interfaces.Validation;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.VerfifyEmployee;

public class VerifyEmployeeValidatorTests
{
    private readonly Mock<IEmployeeChecker> _mockEmployeeChecker;
    private IValidator<VerifyEmployeeCommand>? _validator;
    public VerifyEmployeeValidatorTests()
    {
        _mockEmployeeChecker = new();
    }
    [Fact]
    public async Task VerifyEmployeeValidator_ForValidVerifyEmployeeCommand_ShouldHaveNotErrors()
    {
        //Arrange
            VerifyEmployeeCommand verifyEmployeeCommand = new()
            {
                VerificationToken = "TestToken"
            };
            _mockEmployeeChecker.Setup(x => x.IsVerificationTokenExists(It.IsAny<string>())).Returns(true);
            _validator = new VerifyEmployeeValidator(_mockEmployeeChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(verifyEmployeeCommand);
        //Assert
            result.ShouldNotHaveAnyValidationErrors();
    }
    [Theory]
    [ClassData(typeof(VerifyEmployeeTestsInvalidDataProvider))]
    public async Task VerifyEmployeeValidator_ForInvalidVerifyEmployeeCommand_ShouldHaveErrors(VerifyEmployeeCommand verifyEmployeeCommand)
    {
        //Arange
            _mockEmployeeChecker.Setup(x => x.IsVerificationTokenExists(It.IsAny<string>())).Returns(true);
            _validator = new VerifyEmployeeValidator(_mockEmployeeChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(verifyEmployeeCommand);
        //Assert
            result.ShouldHaveAnyValidationError();
    }
    [Fact]
    public async Task VerifyEmployeeValidator_ForNonExistingVerificationToken_ShouldHaveNotErrorsForVerificationToken()
    {
        //Arrange
            VerifyEmployeeCommand verifyEmployeeCommand = new()
            {
                VerificationToken = "TestToken"
            };
            _mockEmployeeChecker.Setup(x => x.IsVerificationTokenExists(It.IsAny<string>())).Returns(false);
            _validator = new VerifyEmployeeValidator(_mockEmployeeChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(verifyEmployeeCommand);
        //Assert
            result.ShouldHaveValidationErrorFor(x => x.VerificationToken);
    }
}