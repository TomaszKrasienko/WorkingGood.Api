using Application.CQRS.EmployeesAuth.Commands.ResetPassword;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Validation;
using Domain.Models.Employee;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using WorkingGood.Log;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.ResetPassword;

public class ResetPasswordCommandHandlerTests
{
    private readonly Mock<IWgLog<ResetPasswordCommandHandler>> _mockLogger; 
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly Mock<IEmployeeChecker> _mockEmployeeChecker;
    public ResetPasswordCommandHandlerTests()
    {
        _mockLogger = new();
        _mockUnitOfWork = new();
        _mockEmployeeRepository = new();
        _mockUnitOfWork.Setup(x => x.EmployeeRepository)
            .Returns(_mockEmployeeRepository.Object);
        _mockEmployeeChecker = new();
    }

    [Fact]
    public async Task Handle_ForValidResetPasswordCommand_ShouldReturnBaseMessageWithMessage()
    {
        //Arrange
        Employee employee = new Employee(
            "name",
            "lastName",
            "email@email.pl",
            "Test123!",
            Guid.NewGuid());
        employee.SetResetToken();
        ResetPasswordCommand resetPasswordCommand = new()
        {
            ResetPasswordDto = new()
            {
                NewPassword = "Test321!",
                ConfirmNewPassword = "Test321!",
                ResetToken = employee.ResetToken!.Token
            }
        };
        _mockEmployeeChecker.Setup(x => x.IsResetTokenExists(It.IsAny<string>()))
            .Returns(true);
        _mockEmployeeRepository.Setup(x => x.GetByResetToken(It.IsAny<string>()))
            .ReturnsAsync(employee);
        IValidator<ResetPasswordCommand> validator = new ResetPasswordValidator(_mockEmployeeChecker.Object);
        ResetPasswordCommandHandler resetPasswordCommandHandler =
            new ResetPasswordCommandHandler(_mockLogger.Object, _mockUnitOfWork.Object, validator);
        //Act
        var result = await resetPasswordCommandHandler.Handle(resetPasswordCommand, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().Be("The password has been changed");
        result.Errors.Should().BeNull();
    }
    [Theory]
    [ClassData(typeof(ResetPasswordTestsDataProvider))]
    public async Task Handle_ForInvalidResetPasswordCommand_ShouldReturnBaseMessageWithMessage(ResetPasswordCommand resetPasswordCommand)
    {
        //Arrange
        Employee employee = new Employee(
            "name",
            "lastName",
            "email@email.pl",
            "Test123!",
            Guid.NewGuid());
        employee.SetResetToken();
        _mockEmployeeChecker.Setup(x => x.IsResetTokenExists(It.IsAny<string>()))
            .Returns(true);
        IValidator<ResetPasswordCommand> validator = new ResetPasswordValidator(_mockEmployeeChecker.Object);
        ResetPasswordCommandHandler resetPasswordCommandHandler =
            new ResetPasswordCommandHandler(_mockLogger.Object, _mockUnitOfWork.Object, validator);
        //Act
        var result = await resetPasswordCommandHandler.Handle(resetPasswordCommand, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().BeNull();
        result.Errors.Should().NotBeNull();
    }
}