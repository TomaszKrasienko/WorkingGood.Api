using Application.CQRS.EmployeesAuth.Commands.ForgotPassword;
using Application.DTOs;
using Application.Tests.Helpers;
using Domain.Interfaces;
using Domain.Interfaces.Communication;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Validation;
using Domain.Models.Employee;
using FluentAssertions;
using FluentValidation;
using Infrastructure.Common.ConfigModels;
using Infrastructure.Validation;
using Microsoft.Extensions.Logging;
using Moq;
using NLog;
using WorkingGood.Log;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandlerTests
{
    private readonly Mock<IWgLog<ForgotPasswordCommandHandler>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IBrokerSender> _mockBrokerSender;
    private readonly Mock<IEmployeeChecker> _mockEmployeeChecker;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private AddressesConfig? _addressesConfig;
    public ForgotPasswordCommandHandlerTests()
    {
        _mockLogger = new ();
        _mockUnitOfWork = new();
        _mockBrokerSender = new();
        _mockEmployeeChecker = new();
        _mockEmployeeRepository = new();
        _mockUnitOfWork.Setup(x => x.EmployeeRepository).Returns(_mockEmployeeRepository.Object);
    }

    [Theory]
    [ClassData(typeof(ForgotPasswordCommandValidDataProvider))]
    public async Task Handle_ForValidForgotPasswordCommand_ShouldReturnBaseMessageDtoWithMessage(ForgotPasswordCommand passwordCommand)
    { 
        //Arrange
        Employee employee = ObjectProvider.GetEmployee();
        _mockEmployeeChecker
            .Setup(x => x.IsEmployeeExists(It.IsAny<string>()))
            .Returns(true);
        IValidator<ForgotPasswordCommand> validator = new ForgotPasswordValidator(_mockEmployeeChecker.Object);
        _mockEmployeeRepository
            .Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(employee);
        _addressesConfig = new()
        {
            ForgotPasswordUrl = "tmp"
        };
        ForgotPasswordCommandHandler forgotPasswordCommandHandler = new(_mockLogger.Object, _mockUnitOfWork.Object, validator, _mockBrokerSender.Object, _addressesConfig);
        //Act
        var result = await forgotPasswordCommandHandler.Handle(passwordCommand, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().Be("Message sent to employee email");
        result.Errors.Should().BeNull();
    }
    [Theory]
    [ClassData(typeof(ForgotPasswordCommandInvalidDataProvider))]
    public async Task Handle_ForInvalidForgotPasswordCommand_ShouldReturnBaseMessageDtoWithErrorsMessage(ForgotPasswordCommand forgotPasswordCommand)
    { 
        //Arrange
        Employee employee = ObjectProvider.GetEmployee();
        _mockEmployeeChecker
            .Setup(x => x.IsEmployeeExists(It.IsAny<string>()))
            .Returns(true);
        IValidator<ForgotPasswordCommand> validator = new ForgotPasswordValidator(_mockEmployeeChecker.Object);
        _mockEmployeeRepository.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(employee);
        _addressesConfig = new()
        {
            ForgotPasswordUrl = "tmp"
        };
        ForgotPasswordCommandHandler forgotPasswordCommandHandler = new(_mockLogger.Object, _mockUnitOfWork.Object, validator, _mockBrokerSender.Object, _addressesConfig);
        //Act
        var result = await forgotPasswordCommandHandler.Handle(forgotPasswordCommand, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().BeNull();
        result.Errors.Should().NotBeNull();
    }
}