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
    [Fact]
    public async Task ForgotPasswordValidator_ForValidForgotPasswordCommand_ShouldHaveNotErrors()
    {
        //Arrange 
            ForgotPasswordCommand forgotPasswordCommand = new()
            {
                ForgotPasswordDto = new()
                {
                    EmployeeEmail = "test@test.pl"
                }
            };
            _mockEmployeeChecker.Setup(x => x.IsEmployeeExists(It.IsAny<string>())).Returns(true);
            _validator = new ForgotPasswordValidator(_mockEmployeeChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(forgotPasswordCommand);
        //Assert
            result.ShouldNotHaveAnyValidationErrors();
    }

    private static IEnumerable<object[]> GetInvalidForgotPasswordCommand()
    {
        List<ForgotPasswordCommand> forgotPasswordCommands = new()
        {
            new ForgotPasswordCommand()
            {
                ForgotPasswordDto = new()
            },
            new ForgotPasswordCommand()
            {
                
            },
            new ForgotPasswordCommand()
            {
                ForgotPasswordDto = new()
                {
                    EmployeeEmail = "notemail"
                }
            }
        };
        return forgotPasswordCommands.Select(x => new object[] {x});
    }
    [Theory]
    [MemberData(nameof(GetInvalidForgotPasswordCommand))]
    public async Task ForgotPasswordValidator_ForInvalidEmployeeEmail_ShouldHaveErrorForEmployeeEmail(ForgotPasswordCommand forgotPasswordCommand)
    {
        //Arrange
            _mockEmployeeChecker.Setup(x => x.IsEmployeeExists(It.IsAny<string>())).Returns(true);
            _validator = new ForgotPasswordValidator(_mockEmployeeChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(forgotPasswordCommand);
        //Assert
            result.ShouldHaveValidationErrorFor(x => x.ForgotPasswordDto.EmployeeEmail);
    }
    [Fact]
    public async Task ForgotPasswordValidator_ForNonExistingEmployee_ShouldHaveErrorForEmployeeEmail()
    {
        //Arrange
            ForgotPasswordCommand forgotPasswordCommand = new()
            {
                ForgotPasswordDto = new()
                {
                    EmployeeEmail = "test@test.pl"
                }
            };
            _mockEmployeeChecker.Setup(x => x.IsEmployeeExists(It.IsAny<string>())).Returns(false);
            _validator = new ForgotPasswordValidator(_mockEmployeeChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(forgotPasswordCommand);
        //Assert
            result.ShouldHaveValidationErrorFor(x => x.ForgotPasswordDto.EmployeeEmail);
    }

}