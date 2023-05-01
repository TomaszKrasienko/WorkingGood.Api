using Application.CQRS.EmployeesAuth.Commands.RegisterEmployee;
using Application.DTOs;
using Application.EmployeesAuth.Commands;
using Domain.Interfaces;
using Domain.Interfaces.Communication;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Validation;
using Domain.Models.Employee;
using FluentAssertions;
using FluentValidation;
using Infrastructure.Common.ConfigModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.RegisterEmployee;

public class RegisterEmployeeCommandHandlerTests
{
    private readonly Mock<ILogger<RegisterEmployeeCommandHandler>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepositor;
    private readonly Mock<IBrokerSender> _brokerSender;
    private readonly Mock<IEmployeeChecker> _mockEmployeeChecker;
    private readonly Mock<ICompanyChecker> _mockCompanyChecker;
    private readonly Mock<AddressesConfig> _mockAddressesConfig;
    public RegisterEmployeeCommandHandlerTests()
    {
        _mockLogger = new ();
        _mockUnitOfWork = new ();
        _mockEmployeeRepositor = new ();
        _mockUnitOfWork.Setup(x => x.EmployeeRepository)
            .Returns(_mockEmployeeRepositor.Object);
        _brokerSender = new();
        _mockEmployeeChecker = new();
        _mockCompanyChecker = new();
        _mockAddressesConfig = new();
    }

    [Fact]
    public async Task Handle_ForValidRegisterEmployeeCommand_ShouldReturnBaseMessageDtoWithObjectEmployeeIdAndMessage()
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
                    LastName = "TestTest"
                }
            };
            _mockEmployeeChecker.Setup(x => x.IsEmployeeExists(It.IsAny<string>())).Returns(false);
            _mockCompanyChecker.Setup(x => x.IsCompanyExists(It.IsAny<Guid>())).Returns(true);
            _mockAddressesConfig.Setup(x => x.RegistrationUrl).Returns("tmp");
            IValidator<RegisterEmployeeCommand> validator = new RegisterEmployeeValidator(_mockEmployeeChecker.Object, _mockCompanyChecker.Object);
            RegisterEmployeeCommandHandler registerEmployeeCommandHandler = new(_mockLogger.Object, _mockUnitOfWork.Object, validator, _brokerSender.Object, _mockAddressesConfig.Object);
        //Act
            var result = await registerEmployeeCommandHandler.Handle(registerEmployeeCommand, new CancellationToken());
        //Assert
            result.Should().BeOfType<BaseMessageDto>();
            result.Object.Should().BeOfType<Guid>();
            result.Errors.Should().BeNull();
            result.Message.Should().Be("Added employee successfully");
    }
    [Theory]
    [ClassData(typeof(RegisterEmployeeTestsDataProvider))]
    public async Task Handle_ForInvalidRegisterEmployeeCommand_ShouldReturnBaseMessageDtoWithObjectEmployeeIdAndMessage(RegisterEmployeeCommand registerEmployeeCommand)
    {
        //Arrange
            _mockEmployeeChecker.Setup(x => x.IsEmployeeExists(It.IsAny<string>())).Returns(false);
            _mockCompanyChecker.Setup(x => x.IsCompanyExists(It.IsAny<Guid>())).Returns(true);
            _mockAddressesConfig.Setup(x => x.RegistrationUrl).Returns("tmp");
            IValidator<RegisterEmployeeCommand> validator = new RegisterEmployeeValidator(_mockEmployeeChecker.Object, _mockCompanyChecker.Object);
            RegisterEmployeeCommandHandler registerEmployeeCommandHandler = new(_mockLogger.Object, _mockUnitOfWork.Object, validator, _brokerSender.Object, _mockAddressesConfig.Object);
        //Act
            var result = await registerEmployeeCommandHandler.Handle(registerEmployeeCommand, new CancellationToken());
        //Assert
            result.Should().BeOfType<BaseMessageDto>();
            result.Object.Should().BeNull();
            result.Errors.Should().NotBeNull();
            result.Message.Should().BeNull();
    }
}