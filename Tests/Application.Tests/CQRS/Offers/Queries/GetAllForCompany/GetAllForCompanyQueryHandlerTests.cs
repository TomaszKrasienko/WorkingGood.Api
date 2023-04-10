using Application.CQRS.Offers.Queries.GetAllForCompany;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models.Employee;
using Domain.Models.Offer;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using NLog;

namespace Application.Tests.CQRS.Offers.Queries.GetAllForCompany;

public class GetAllForCompanyQueryHandlerTests
{
    private readonly Mock<ILogger<GetAllForCompanyQueryHandler>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IOfferRepository> _mockOfferRepository;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    public GetAllForCompanyQueryHandlerTests()
    {
        _mockLogger = new();
        _mockUnitOfWork = new();
        _mockOfferRepository = new();
        _mockEmployeeRepository = new();
        _mockUnitOfWork.Setup(x => x.OffersRepository)
            .Returns(_mockOfferRepository.Object);
        _mockUnitOfWork.Setup(x => x.EmployeeRepository)
            .Returns(_mockEmployeeRepository.Object);
    }

    [Fact]
    public async Task Handle_ForValidGetAllForCompanyQueryAndExistsOffer_ShouldReturnOkObjectResult()
    {
        //Arrange 
        Employee employee = new Employee("Test", "Test", "Test", "Test123!", Guid.NewGuid());
        _mockEmployeeRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(employee);
        _mockEmployeeRepository
            .Setup(x => x.GetByCompanyIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new List<Employee>()
            {
                employee   
            });
        Offer offer = new(
            "Title test",
            "Test postition type",
            10000,
            15000,
            "Description test Description test Description test",
            Guid.NewGuid()
            );
        _mockOfferRepository
            .Setup(x => x.GetAllForEmployees(It.IsAny<List<Guid>>()))
            .ReturnsAsync(new List<Offer>()
            {
                offer
            });
        IValidator<GetAllForCompanyQuery> _validator = new GetAllForEmployeeValidator();
        GetAllForCompanyQueryHandler getAllForCompanyQueryHandler = new(_mockLogger.Object, _mockUnitOfWork.Object, _validator);
        GetAllForCompanyQuery getAllForCompanyQuery = new()
        {
            EmployeeId = Guid.NewGuid()
        };
        //Act
        var result = await getAllForCompanyQueryHandler.Handle(getAllForCompanyQuery, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Object.Should().BeOfType<List<Offer>>();
    }
}