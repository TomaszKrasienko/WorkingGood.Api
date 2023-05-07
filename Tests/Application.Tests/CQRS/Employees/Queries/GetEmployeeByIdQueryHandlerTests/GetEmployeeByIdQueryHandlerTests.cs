using Application.CQRS.Employees.Queries;
using Application.DTOs;
using Application.ViewModels.GetEmployee;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models.Employee;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
namespace Application.Tests.CQRS.Employees.Queries.GetEmployeeByIdQueryHandlerTests;

public class GetEmployeeByIdQueryHandlerTests
{
    private readonly Mock<ILogger<GetEmployeeByIdQueryHandler>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly Mock<IMapper> _mockMapper;
    public GetEmployeeByIdQueryHandlerTests()
    {
        _mockLogger = new();
        _mockUnitOfWork = new();
        _mockEmployeeRepository = new();
        _mockUnitOfWork
            .Setup(x => x.EmployeeRepository)
            .Returns(_mockEmployeeRepository.Object);
        _mockMapper = new();
    }
    [Fact]
    public async Task Handle_ForValidGetEmployeeByIdQuery_ShouldReturnBaseMessageDto()
    {
        //Arrange
        Employee employee = new Employee("Test", "Test", $"{Guid.NewGuid()}@test.pl", "Test123!", Guid.NewGuid());
        _mockEmployeeRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(employee);
        _mockMapper
            .Setup(x => x.Map<GetEmployeeVM>(It.IsAny<Employee>()))
            .Returns(new GetEmployeeVM()
            {
                Email = "test@test.pl",
                FirstName = "Test",
                LastName = "Test",
                IsActive = true
            });
        IValidator<GetEmployeeByIdQuery> validator = new GetEmployeeByIdValidator();
        var getEmployeeByIdQueryHandler = new GetEmployeeByIdQueryHandler(
            _mockLogger.Object,
            _mockUnitOfWork.Object,
            validator,
            _mockMapper.Object
        );
        //Act
        var result = await getEmployeeByIdQueryHandler.Handle(new GetEmployeeByIdQuery()
        {
            Id = Guid.NewGuid()
        }, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Errors.Should().BeNull();
        result.Object.Should().BeOfType<GetEmployeeVM>();
    }
}