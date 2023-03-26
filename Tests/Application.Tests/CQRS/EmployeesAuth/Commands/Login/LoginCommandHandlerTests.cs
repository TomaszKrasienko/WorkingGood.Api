using Application.CQRS.EmployeesAuth.Commands.Login;
using Application.DTOs;
using Application.ViewModels.Login;
using Domain.Common.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Validation;
using Domain.Models.Employee;
using FluentAssertions;
using FluentValidation;
using Infrastructure.Common.ConfigModels;
using Infrastructure.Persistance;
using Microsoft.Extensions.Logging;
using Moq;
using NLog;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.Login;

public class LoginCommandHandlerTests
{
    private readonly Mock<ILogger<LoginCommandHandler>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly Mock<IEmployeeChecker> _mockEmployeeChecker;
    private readonly JwtConfig _jwtConfig;
    public LoginCommandHandlerTests()
    {
        _mockLogger = new();
        _mockUnitOfWork = new();
        _mockEmployeeRepository = new();
        _mockUnitOfWork.Setup(x => x.EmployeeRepository).Returns(_mockEmployeeRepository.Object);
        _mockEmployeeChecker = new();
        _jwtConfig = new JwtConfig()
        {
            TokenKey = "my top secret key",
            Audience = "my_secret_audience",
            Issuer = "my_secret_issuer"
        };
    }
    [Fact]
    public async Task Handle_ForValidLoginCommand_ShouldReturnBaseMessageDtoWithMessageAndLoginVmObject()
    {
        //Arrange
            string empPass = "Test123";
            LoginCommand loginCommand = new()
            {
                CredentialsDto = new()
                {
                    Email = "test@test.pl",
                    Password = empPass
                }
            };
            _mockEmployeeChecker.Setup(x => x.IsEmployeeExists(It.IsAny<string>())).Returns(true);
            IValidator<LoginCommand> validator = new LoginValidator(_mockEmployeeChecker.Object);
            Employee employee = new Employee("name", "lastName", "test@test.pl", empPass, Guid.NewGuid());
            employee.Activate();
            _mockEmployeeRepository.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(employee);
            LoginCommandHandler loginCommandHandler =
                new LoginCommandHandler(_mockLogger.Object, _jwtConfig, _mockUnitOfWork.Object, validator);
        //Act
            var result = await loginCommandHandler.Handle(loginCommand, new CancellationToken());
        //Assert
            result.Should().BeOfType<BaseMessageDto>();
            result.Errors.Should().BeNull();
            result.Message.Should().Be("Login successfully");
            result.Object.Should().BeOfType<LoginVM>();
    }   
    [Fact]
    public async Task Handle_ForValidLoginCommandAndNotActiveEmployee_ShouldReturnBaseMessageDtoWithMessageAndLoginVmObject()
    {
        //Arrange
        string empPass = "Test123";
        LoginCommand loginCommand = new()
        {
            CredentialsDto = new()
            {
                Email = "test@test.pl",
                Password = empPass
            }
        };
        _mockEmployeeChecker.Setup(x => x.IsEmployeeExists(It.IsAny<string>())).Returns(true);
        IValidator<LoginCommand> validator = new LoginValidator(_mockEmployeeChecker.Object);
        Employee employee = new Employee("name", "lastName", "test@test.pl", empPass, Guid.NewGuid());
        _mockEmployeeRepository.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(employee);
        LoginCommandHandler loginCommandHandler =
            new LoginCommandHandler(_mockLogger.Object, _jwtConfig, _mockUnitOfWork.Object, validator);
        //Act
        Func<Task> result = async() => await loginCommandHandler.Handle(loginCommand, new CancellationToken());
        //Assert
        await result.Should().ThrowExactlyAsync<LoginException>().WithMessage("Employee is not active");
    }
}