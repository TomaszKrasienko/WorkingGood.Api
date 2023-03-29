using Application.CQRS.EmployeesAuth.Commands.VerifyEmployee;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Validation;
using Domain.Models.Employee;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.VerfifyEmployee;

public class VerifyEmployeeCommandHandlerTests
{
    private readonly Mock<ILogger<VerifyEmployeeCommandHandler>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly Mock<IEmployeeChecker> _mockEmployeeChecker;
    public VerifyEmployeeCommandHandlerTests()
    {
        _mockLogger = new();
        _mockUnitOfWork = new();
        _mockEmployeeRepository = new();
        _mockUnitOfWork.Setup(x => x.EmployeeRepository).Returns(_mockEmployeeRepository.Object);
        _mockEmployeeChecker = new();
    }

    [Fact]
    public async Task Handle_ForValidVerifyEmployeeCommand_ShouldReturnBaseMessageDtoWithMessage()
    {
        //Arrange
            Employee employee = new("Test", "Test", "Test@Test.pl", "Test123", Guid.NewGuid());
            _mockEmployeeRepository.Setup(x => x.GetByVerificationTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(employee);
            _mockEmployeeChecker.Setup(x => x.IsVerificationTokenExists(It.IsAny<string>())).Returns(true);
            IValidator<VerifyEmployeeCommand> validator = new VerifyEmployeeValidator(_mockEmployeeChecker.Object);
            VerifyEmployeeCommandHandler verifyEmployeeCommandHandler =
                new(_mockLogger.Object, _mockUnitOfWork.Object, validator);
            VerifyEmployeeCommand verifyEmployeeCommand = new()
            {
                VerificationToken = "TestTest"
            };
        //Act
            var result = await verifyEmployeeCommandHandler.Handle(verifyEmployeeCommand, new CancellationToken());
        //Assert
            result.Should().BeOfType<BaseMessageDto>();
            result.Errors.Should().BeNull();
            result.Message.Should().Be("Account activated successfully");
    }

    [Theory]
    [ClassData(typeof(VerifyEmployeeTestsInvalidDataProvider))]
    public async Task Handle_ForInvalidVerifyEmployeeCommand_ShouldReturnBaseMessageDtoWithErrorMessage(VerifyEmployeeCommand verifyEmployeeCommand)
    {
        //Arrange
            Employee employee = new("Test", "Test", "Test@Test.pl", "Test123", Guid.NewGuid());
            _mockEmployeeRepository.Setup(x => x.GetByVerificationTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(employee);
            _mockEmployeeChecker.Setup(x => x.IsVerificationTokenExists(It.IsAny<string>())).Returns(true);
            IValidator<VerifyEmployeeCommand> validator = new VerifyEmployeeValidator(_mockEmployeeChecker.Object);
            VerifyEmployeeCommandHandler verifyEmployeeCommandHandler =
                new(_mockLogger.Object, _mockUnitOfWork.Object, validator);
        //Act
            var result = await verifyEmployeeCommandHandler.Handle(verifyEmployeeCommand, new CancellationToken());
        //Assert
            result.Should().BeOfType<BaseMessageDto>();
            result.Message.Should().BeNullOrEmpty();
            result.Errors.Should().NotBeNull();
    }
}