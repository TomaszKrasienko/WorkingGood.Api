using Application.CQRS.Offers.Queries.GetActiveOffers;
using Application.DTOs;
using Application.ViewModels.GetEmployee;
using Application.ViewModels.Offer;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models.Offer;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.CQRS.Offers.Queries.GetActiveOffers;

public class GetActiveOffersQueryHandlerTests
{
    private readonly Mock<ILogger<GetActiveOffersQueryHandler>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IOfferRepository> _mockOfferRepository;
    private readonly Mock<IMapper> _mockMapper;
    public GetActiveOffersQueryHandlerTests()
    {
        _mockLogger = new();
        _mockUnitOfWork = new();
        _mockOfferRepository = new();
        _mockMapper = new();
        _mockUnitOfWork
            .Setup(x => x.OffersRepository)
            .Returns(_mockOfferRepository.Object);
    }

    [Fact]
    public async Task Handle_ForGetActiveQuery_ShouldReturnBaseResponseWithObjectOfListGetOfferVms()
    {
        //Arrange
        List<Offer> offersList = new()
        {
            new("titleTest", "positionTypeTest", 10000, 15000,
                "descriptionTestDescriptionTestDescriptionTestDescriptionTestDescriptionTest", Guid.NewGuid(), true),
            new("titleTest1", "positionTypeTest1", 10000, 15000,
                "descriptionTestDescriptionTestDescriptionTestDescriptionTestDescriptionTest", Guid.NewGuid(), true),
        };
        _mockOfferRepository
            .Setup(x => x.GetAllActive())
            .ReturnsAsync(offersList);
        _mockMapper
            .Setup(x => x.Map<GetOfferVM>(offersList[0]))
            .Returns(new GetOfferVM()
            {
                Id = Guid.NewGuid(),
                Description = "descriptionTestDescriptionTestDescriptionTestDescriptionTestDescriptionTest",
                Title = "titleTest",
                IsActive = true,
                PositionType = "positionTypeTest",
                SalaryRangeMax = 15000,
                SalaryRangeMin = 10000
            });
        _mockMapper
            .Setup(x => x.Map<GetOfferVM>(offersList[1]))
            .Returns(new GetOfferVM()
            {
                Id = Guid.NewGuid(),
                Description = "descriptionTestDescriptionTestDescriptionTestDescriptionTestDescriptionTest",
                Title = "titleTest1",
                IsActive = true,
                PositionType = "positionTypeTest1",
                SalaryRangeMax = 15000,
                SalaryRangeMin = 10000
            });
        // _mockMapper
        //     .Setup(x => x.Map<GetOfferVM>(It.Is<Offer>(x => x.Content.Title == offersList[0].Content.Title)))
        //     .Returns(new GetOfferVM()
        //     {
        //         Id = Guid.NewGuid(),
        //         Description = "descriptionTestDescriptionTestDescriptionTestDescriptionTestDescriptionTest",
        //         Title = "titleTest",
        //         IsActive = true,
        //         PositionType = "positionTypeTest",
        //         SalaryRangeMax = 15000,
        //         SalaryRangeMin = 10000
        //     });
        // _mockMapper
        //     .Setup(x => x.Map<GetOfferVM>(It.Is<Offer>(x => x == offersList[1])))
        //     .Returns(new GetOfferVM()
        //     {
        //         Id = Guid.NewGuid(),
        //         Description = "descriptionTestDescriptionTestDescriptionTestDescriptionTestDescriptionTest",
        //         Title = "titleTest1",
        //         IsActive = true,
        //         PositionType = "positionTypeTest1",
        //         SalaryRangeMax = 15000,
        //         SalaryRangeMin = 10000
        //     });
        var getActiveOffersQueryHandler =
            new GetActiveOffersQueryHandler(_mockLogger.Object, _mockUnitOfWork.Object, _mockMapper.Object);
        //Act
        var result = await getActiveOffersQueryHandler.Handle(new GetActiveOffersQuery(), new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Object.Should().BeOfType<List<GetOfferVM>>();
        result.Errors.Should().BeNull();
    }
}