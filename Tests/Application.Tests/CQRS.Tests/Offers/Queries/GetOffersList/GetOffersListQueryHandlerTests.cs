using Application.CQRS.Offers.Queries.GetOffersList;
using Application.DTOs;
using Application.ViewModels.Offer;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models.Employee;
using Domain.Models.Offer;
using FluentAssertions;
using FluentValidation;
using Infrastructure.Persistance;
using Microsoft.Extensions.Logging;
using Moq;
using WorkingGood.Log;

namespace Application.Tests.CQRS.Offers.Queries.GetOffersList;

public class GetOffersListQueryHandlerTests
{
    private readonly Mock<IWgLog<GetOffersListQueryHandler>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
    private readonly Mock<IOfferRepository> _mockOfferRepository;
        private readonly Mock<IMapper> _mockMapper;
    private readonly IValidator<GetOffersListQuery> _validator;
    public GetOffersListQueryHandlerTests()
    {
        _mockLogger = new();
        _mockUnitOfWork = new();
        _mockEmployeeRepository = new();
        _mockOfferRepository = new();
        _mockUnitOfWork
            .Setup(x => x.EmployeeRepository)
            .Returns(_mockEmployeeRepository.Object);
        _mockUnitOfWork
            .Setup(x => x.OffersRepository)
            .Returns(_mockOfferRepository.Object);
        _mockMapper = new();
        _validator = new GetOffersListValidator();
    }

    [Theory]
    [ClassData(typeof(GetOffersListQueryValidDataProvider))]
    public async Task Handle_ForValidGetOffersQuery_ShouldReturnBaseResponseWithListGetOffersVm(GetOffersListQuery getOffersListQuery)
    {
        _mockEmployeeRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Employee("testFirstName", "testLastName", "test@test.pl", "123Test!", Guid.NewGuid()));
        _mockEmployeeRepository
            .Setup(x => x.GetByCompanyIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(
                new List<Employee>()
                {
                    new Employee("testFirstName", "testLastName", "test@test.pl", "123Test!", Guid.NewGuid())
                }
            );
        _mockOfferRepository
            .Setup(x => x.GetAllAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<List<Guid>>(), 
                It.IsAny<bool>(),
                It.IsAny<Guid>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<string>()));
        _mockMapper
            .Setup(x => x.Map<List<GetOfferVM>>(It.IsAny<List<Offer>>()))
            .Returns(new List<GetOfferVM>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Title = "testTile",
                    Description = "testDescription",
                    IsActive = true,
                    PositionType = "testPositionType",
                    SalaryRangeMax = 10000,
                    SalaryRangeMin = 10000
                }
            });
        GetOffersListQueryHandler getOffersListQueryHandler = new(
            _mockLogger.Object,
            _validator,
            _mockUnitOfWork.Object,
            _mockMapper.Object);
        //Act
        var result = await getOffersListQueryHandler.Handle(getOffersListQuery, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Object.Should().BeOfType<List<GetOfferVM>>();
        result.Errors.Should().BeNull();
    }
}