using Application.CQRS.EmployeesAuth.Commands.Refresh;
using Application.DTOs;
using Application.ViewModels.Login;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models.Employee;
using FluentAssertions;
using FluentValidation;
using Infrastructure.Common.ConfigModels;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.Refresh;

public class RefreshCommandHandlerTests
{
    private readonly Mock<ILogger<RefreshCommandHandler>> _mockLogger;
    private readonly JwtConfig _jwtConfig;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;

    public RefreshCommandHandlerTests()
    {
        _mockLogger = new();
        _jwtConfig = new()
        {
            TokenKey = "my top secret key",
            Audience = "my_secret_audience",
            Issuer = "my_secret_issuer"
        };
        _mockUnitOfWork = new();
        _mockEmployeeRepository = new();
        _mockUnitOfWork.Setup(x => x.EmployeeRepository).Returns(_mockEmployeeRepository.Object);
    }

    [Fact]
    public async Task Handle_ForValidRefreshCommandHandler_ShouldReturnBaseMessageDtoWithMessageAndLoginVmObject()
    {
        //Arrange
            RefreshCommand refreshCommand = new()
            {
                RefreshDto = new()
                {
                    RefreshToken = "TestRefreshToken"
                }
            };
            IValidator<RefreshCommand> validator = new RefreshValidator();
            Employee employee = new Employee("name", "lastName", "test@test.pl", "TestPass", Guid.NewGuid());
            employee.Activate();
            _mockEmployeeRepository.Setup(x => x.GetByRefreshTokenAsync(It.IsAny<string>())).ReturnsAsync(employee);
            // RefreshCommandHandler refreshCommandHandler =
            //     new(_mockLogger.Object, _jwtConfig, _mockUnitOfWork.Object, validator);
        //Act
            // var result = await refreshCommandHandler.Handle(refreshCommand, new CancellationToken());
        //Assert
            // result.Should().BeOfType<BaseMessageDto>();
            // result.Message.Should().Be("Login successfully");
            // result.Errors.Should().BeNull();
            // result.Object.Should().BeOfType<LoginVM>();
    }
    [Fact]
    public async Task Handle_ForInvalidRefreshCommandHandler_ShouldReturnBaseMessageDtoWithErrors()
    {
        //Arrange
            RefreshCommand refreshCommand = new()
            {
                RefreshDto = new()
                {
                    RefreshToken = ""
                }
            };
            IValidator<RefreshCommand> validator = new RefreshValidator();
            Employee employee = new Employee("name", "lastName", "test@test.pl", "TestPass", Guid.NewGuid());
            employee.Activate();
            _mockEmployeeRepository.Setup(x => x.GetByRefreshTokenAsync(It.IsAny<string>())).ReturnsAsync(employee);
        //     RefreshCommandHandler refreshCommandHandler =
        //         new(_mockLogger.Object, _jwtConfig, _mockUnitOfWork.Object, validator);
        // //Act
        //     var result = await refreshCommandHandler.Handle(refreshCommand, new CancellationToken());
        // //Assert
        //     result.Should().BeOfType<BaseMessageDto>();
        //     result.Message.Should().BeNull();
        //     result.Errors.Should().NotBeNull();
        //     result.Object.Should().BeNull();
    }
    [Fact]
    public async Task Handle_ForNotExistingRefreshToken_ShouldReturnBaseMessageDtoWithErrors()
    {
        //Arrange
            RefreshCommand refreshCommand = new()
            {
                RefreshDto = new()
                {
                    RefreshToken = "TestRefreshToken"
                }
            };
            IValidator<RefreshCommand> validator = new RefreshValidator();
            Employee employee = new Employee("name", "lastName", "test@test.pl", "TestPass", Guid.NewGuid());
            employee.Activate();
            _mockEmployeeRepository.Setup(x => x.GetByRefreshTokenAsync(It.IsAny<string>()));
        //     RefreshCommandHandler refreshCommandHandler =
        //         new(_mockLogger.Object, _jwtConfig, _mockUnitOfWork.Object, validator);
        // //Act
        //     var result = await refreshCommandHandler.Handle(refreshCommand, new CancellationToken());
        // //Assert
        //     result.Should().BeOfType<BaseMessageDto>();
        //     result.Message.Should().BeNull();
        //     result.Errors.Should().NotBeNull();
        //     result.Object.Should().BeNull();
    }
}