using Application.CQRS.EmployeesAuth.Commands.Refresh;
using Application.DTOs;
using Application.DTOs.EmployeesAuth;
using Application.ViewModels.Login;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models.Employee;
using Domain.Services;
using Domain.ValueObjects;
using FluentAssertions;
using FluentValidation;
using Infrastructure.Common.ConfigModels;
using Microsoft.Extensions.Logging;
using Moq;
using WorkingGood.Log;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.Refresh;

public class RefreshCommandHandlerTests
{
    private readonly Mock<IWgLog<RefreshCommandHandler>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly Mock<ITokenProvider> _mockTokenProvider;
    public RefreshCommandHandlerTests()
    {
        _mockLogger = new();
        _mockUnitOfWork = new();
        _mockEmployeeRepository = new();
        _mockUnitOfWork.Setup(x => x.EmployeeRepository).Returns(_mockEmployeeRepository.Object);
        _mockTokenProvider = new();
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
            _mockEmployeeRepository
                .Setup(x => x.GetByRefreshTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(employee);
            _mockTokenProvider
                .Setup(x => x.Provide(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new LoginToken()
                {
                    Expiration = DateTime.Now,
                    Token = "test string token"
                });
             RefreshCommandHandler refreshCommandHandler =
                 new(_mockLogger.Object, validator, _mockUnitOfWork.Object, _mockTokenProvider.Object);
        //Act
             var result = await refreshCommandHandler.Handle(refreshCommand, new CancellationToken());
        //Assert
             result.Should().BeOfType<RefreshResponseDto>();
             result.Message.Should().Be("Login successfully");
             result.Errors.Should().BeNull();
             result.Object.Should().BeOfType<LoginVM>();
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
            _mockEmployeeRepository
                .Setup(x => x.GetByRefreshTokenAsync(It.IsAny<string>()))
                .ReturnsAsync(employee);
            _mockTokenProvider
                .Setup(x => x.Provide(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new LoginToken()
                {
                    Expiration = DateTime.Now,
                    Token = "test string token"
                });
            RefreshCommandHandler refreshCommandHandler =
                new(_mockLogger.Object, validator, _mockUnitOfWork.Object, _mockTokenProvider.Object);
        //Act
            var result = await refreshCommandHandler.Handle(refreshCommand, new CancellationToken());
        //Assert
            result.Should().BeOfType<RefreshResponseDto>();
            result.Message.Should().BeNull();
            result.Errors.Should().NotBeNull();
            result.Object.Should().BeNull();
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
            _mockEmployeeRepository
                .Setup(x => x.GetByRefreshTokenAsync(It.IsAny<string>()));
            _mockTokenProvider
                .Setup(x => x.Provide(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new LoginToken()
                {
                    Expiration = DateTime.Now,
                    Token = "test string token"
                });
            RefreshCommandHandler refreshCommandHandler =
                new(_mockLogger.Object, validator, _mockUnitOfWork.Object, _mockTokenProvider.Object);
        //Act
            var result = await refreshCommandHandler.Handle(refreshCommand, new CancellationToken());
        //Assert
            result.Should().BeOfType<RefreshResponseDto>();
            result.Message.Should().BeNull();
            result.Errors.Should().NotBeNull();
            result.Object.Should().BeNull();
    }
}