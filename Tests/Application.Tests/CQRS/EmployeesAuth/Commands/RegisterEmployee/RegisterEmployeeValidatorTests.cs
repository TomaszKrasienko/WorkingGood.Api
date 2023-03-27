using Application.CQRS.EmployeesAuth.Commands.RegisterEmployee;
using Application.EmployeesAuth.Commands;
using Domain.Interfaces.Validation;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.RegisterEmployee;

public class RegisterEmployeeValidatorTests
{
    private readonly Mock<IEmployeeChecker> _mockEmployeeChecker;
    private readonly Mock<ICompanyChecker> _mockCompanyChecker;
    private IValidator<RegisterEmployeeCommand>? _validator;
    public RegisterEmployeeValidatorTests()
    {
        _mockEmployeeChecker = new();
        _mockCompanyChecker = new();
    }
    [Fact]
    public async Task RegisterEmployeeValidator_ForValidRegisterEmployeeCommand_ShouldHaveNotErrors()
    {
        //Arrange
            RegisterEmployeeCommand registerEmployeeCommand = new()
            {
                CompanyId = Guid.NewGuid(),
                RegisterEmployeeDto = new()
                {
                    Email = "test@test.pl",
                    Password = "Test123",
                    FirstName = "Test",
                    LastName = "Test"
                }
            };
            _mockEmployeeChecker.Setup(x => x.IsEmployeeExists(It.IsAny<string>())).Returns(false);
            _mockCompanyChecker.Setup(x => x.IsCompanyExists(It.IsAny<Guid>())).Returns(true);
            _validator = new RegisterEmployeeValidator(_mockEmployeeChecker.Object, _mockCompanyChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(registerEmployeeCommand);
        //Assert
            result.ShouldNotHaveAnyValidationErrors();
    }
    [Theory]
    [ClassData(typeof(RegisterEmployeeTestsDataProvider))]
    public async Task RegisterEmployeeValidator_ForInvalidRegisterEmployeeCommand_ShouldHaveNotErrors(RegisterEmployeeCommand registerEmployeeCommand)
    {
        //Arrange
            _mockEmployeeChecker.Setup(x => x.IsEmployeeExists(It.IsAny<string>())).Returns(false);
            _mockCompanyChecker.Setup(x => x.IsCompanyExists(It.IsAny<Guid>())).Returns(true);
            _validator = new RegisterEmployeeValidator(_mockEmployeeChecker.Object, _mockCompanyChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(registerEmployeeCommand);
        //Assert
            result.ShouldHaveAnyValidationError();
    }
    [Fact]
    public async Task RegisterEmployeeValidator_ForNotExistingCompany_ShouldHaveErrorsForCompanyId()
    {
        //Arrange
            RegisterEmployeeCommand registerEmployeeCommand = new()
            {
                CompanyId = Guid.NewGuid(),
                RegisterEmployeeDto = new()
                {
                    Email = "test@test.pl",
                    Password = "Test123",
                    FirstName = "Test",
                    LastName = "Test"
                }
            };
            _mockEmployeeChecker.Setup(x => x.IsEmployeeExists(It.IsAny<string>())).Returns(false);
            _mockCompanyChecker.Setup(x => x.IsCompanyExists(It.IsAny<Guid>())).Returns(false);
            _validator = new RegisterEmployeeValidator(_mockEmployeeChecker.Object, _mockCompanyChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(registerEmployeeCommand);
        //Assert
            result.ShouldHaveValidationErrorFor(x => x.CompanyId);
    }
    [Fact]
    public async Task RegisterEmployeeValidator_ForExistingEmployee_ShouldHaveErrorsForEmployeeEmail()
    {
        //Arrange
            RegisterEmployeeCommand registerEmployeeCommand = new()
            {
                CompanyId = Guid.NewGuid(),
                RegisterEmployeeDto = new()
                {
                    Email = "test@test.pl",
                    Password = "Test123",
                    FirstName = "Test",
                    LastName = "Test"
                }
            };
            _mockEmployeeChecker.Setup(x => x.IsEmployeeExists(It.IsAny<string>())).Returns(true);
            _mockCompanyChecker.Setup(x => x.IsCompanyExists(It.IsAny<Guid>())).Returns(true);
            _validator = new RegisterEmployeeValidator(_mockEmployeeChecker.Object, _mockCompanyChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(registerEmployeeCommand);
        //Assert
            result.ShouldHaveValidationErrorFor(x => x.RegisterEmployeeDto.Email);
    }
}