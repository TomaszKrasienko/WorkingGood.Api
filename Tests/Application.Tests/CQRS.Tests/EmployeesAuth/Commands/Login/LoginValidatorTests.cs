using Application.CQRS.EmployeesAuth.Commands.ForgotPassword;
using Application.CQRS.EmployeesAuth.Commands.Login;
using Domain.Interfaces.Validation;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.Login;

public class LoginValidatorTests
{
    private IValidator<LoginCommand>? _validator;
    private readonly Mock<IEmployeeChecker> _mockEmployeeChecker;
    public LoginValidatorTests()
    {
        _mockEmployeeChecker = new();
    }

    [Fact]
    public async Task LoginValidator_ForValidLoginCommand_ShouldHaveNotErrors()
    {
        //Arrange
            LoginCommand loginCommand = new()
            {
                CredentialsDto = new()
                {
                    Email = "tom@tom.pl",
                    Password = "Test123"
                }
            };
            _mockEmployeeChecker.Setup(x => x.IsEmployeeExists(It.IsAny<string>())).Returns(true);
            _validator = new LoginValidator(_mockEmployeeChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(loginCommand);
        //Assert
            result.ShouldNotHaveAnyValidationErrors();
    }

    private static IEnumerable<object[]> GetLoginCommandsWithInvalidCredentials()
    {
        List<LoginCommand> loginCommands = new()
        {
            new LoginCommand()
            {
                CredentialsDto = new()
            },
            new LoginCommand(),
            new LoginCommand()
            {
                CredentialsDto = new()
                {
                    Password = "123"
                }
            },
            new LoginCommand()
            {
                CredentialsDto = new()
                {
                    Email = "test@test.pl"
                }
            },
            new LoginCommand()
            {
                CredentialsDto = new()
                {
                    Email = "",
                    Password = ""
                }
            }
        };
        return loginCommands.Select(x => new object[] {x});
    }
    [Theory]
    [MemberData(nameof(GetLoginCommandsWithInvalidCredentials))]
    public async Task LoginValidator_ForInvalidCredentialsDto_ShouldHaveErrors(LoginCommand loginCommand)
    {
        //Arrange
            _mockEmployeeChecker.Setup(x => x.IsEmployeeExists(It.IsAny<string>())).Returns(true);
            _validator = new LoginValidator(_mockEmployeeChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(loginCommand);
        //Assert
            result.ShouldHaveAnyValidationError();
    }
}