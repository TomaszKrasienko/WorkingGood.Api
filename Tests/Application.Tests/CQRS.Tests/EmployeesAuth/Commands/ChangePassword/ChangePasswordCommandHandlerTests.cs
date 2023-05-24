using Application.CQRS.Companies.Commands;
using Application.CQRS.EmployeesAuth.Commands.ChangePassword;
using Application.DTOs;
using Application.DTOs.EmployeesAuth;
using Application.Tests.Helpers;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Validation;
using Domain.Models.Employee;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.ChangePassword;

public class ChangePasswordCommandHandlerTests
{
    private readonly Mock<ILogger<ChangePasswordCommandHandler>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IEmployeeChecker> _mockEmployeeChecker;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    public ChangePasswordCommandHandlerTests()
    {
        _mockLogger = new Mock<ILogger<ChangePasswordCommandHandler>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockEmployeeRepository = new Mock<IEmployeeRepository>();
        _mockEmployeeChecker = new Mock<IEmployeeChecker>();
        _mockUnitOfWork.Setup(x => x.EmployeeRepository).Returns(_mockEmployeeRepository.Object);
    }

    [Theory]
    [ClassData(typeof(ChangePasswordCommandValidDataProvider))]
    public async Task Handle_ForValidChangePasswordCommand_ShouldReturnBaseMessageDtoWithMessage(ChangePasswordCommand changePasswordCommand)
    {
        //Arrange
        Employee employee = ObjectProvider.GetEmployee(changePasswordCommand.ChangePasswordDto.OldPassword);
        _mockEmployeeChecker
            .Setup(x => x.IsEmployeeExists(It.IsAny<Guid>()))
            .Returns(true);
        _mockEmployeeRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(employee);
        IValidator<ChangePasswordCommand> validator = new ChangePasswordValidator(_mockEmployeeChecker.Object);
        var changePasswordCommandHandler = new ChangePasswordCommandHandler(_mockLogger.Object, _mockUnitOfWork.Object, validator);
        //Act
        var result = await changePasswordCommandHandler.Handle(changePasswordCommand, new CancellationToken());    
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().Be("Password changed");
        result.Errors.Should().BeNull();
        result.MetaData.Should().BeNull();
    }
    
    [Theory]
    [ClassData(typeof(ChangePasswordCommandInvalidDataProvider))]
    public async Task Handle_ForInvalidChangePasswordCommand_ShouldReturnBaseMessageDtoWithErrorMessage(ChangePasswordCommand changePasswordCommand)
    {
        //Arrange
        Employee employee = ObjectProvider.GetEmployee();
        _mockEmployeeChecker
            .Setup(x => x.IsEmployeeExists(It.IsAny<Guid>()))
            .Returns(true);
        _mockEmployeeRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(employee);
        IValidator<ChangePasswordCommand> validator = new ChangePasswordValidator(_mockEmployeeChecker.Object);
        var changePasswordCommandHandler =
            new ChangePasswordCommandHandler(_mockLogger.Object, _mockUnitOfWork.Object, validator);
        //Act
        var result = await changePasswordCommandHandler.Handle(changePasswordCommand, new CancellationToken());    
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().BeNull();
        result.Errors.Should().NotBeNull();
    }
    [Fact]
    public async Task Handle_ForNotFoundEmployee_ShouldReturnBaseMessageDtoWithErrorMessage()
    {
        //Arrange
        string empOldPass = "TestOldPass123!";
        Employee employee = new("TestFirstName", "TestLastName", "Test@Test.pl", "OldPass123", Guid.NewGuid());
        _mockEmployeeChecker.Setup(x => x.IsEmployeeExists(It.IsAny<Guid>())).Returns(false);
        _mockEmployeeRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(employee);
        IValidator<ChangePasswordCommand> validator = new ChangePasswordValidator(_mockEmployeeChecker.Object);
        string newPass = "TestPass123";            
        ChangePasswordCommand changePasswordCommand = new ChangePasswordCommand()
        {
            EmployeeId = Guid.NewGuid(),
            ChangePasswordDto = new()
            {
                NewPassword = newPass,
                OldPassword = empOldPass,
                ConfirmNewPassword = newPass
            }
        };
        var changePasswordCommandHandler =
            new ChangePasswordCommandHandler(_mockLogger.Object, _mockUnitOfWork.Object, validator);
        //Act
        var result = await changePasswordCommandHandler.Handle(changePasswordCommand, new CancellationToken());    
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().BeNull();
        result.Errors.Should().NotBeNull();
    }
}