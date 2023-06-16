using Application.CQRS.Offers.Queries.GetOfferStatus;
using Application.CQRS.Offers.Queries.GetStatus;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Validation;
using Domain.Models.Offer;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using WorkingGood.Log;

namespace Application.Tests.CQRS.Offers.Queries.GetStatus;

public class GetStatusQueryHandlerTests
{
    private readonly Mock<IWgLog<GetStatusQuery>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IOfferRepository> _mockOfferRepository;
    private readonly Mock<IOfferChecker> _mockOfferChecker;
    public GetStatusQueryHandlerTests()
    {
        _mockLogger = new();
        _mockUnitOfWork = new();
        _mockOfferRepository = new();
        _mockUnitOfWork.Setup(x => x.OffersRepository)
            .Returns(_mockOfferRepository.Object);
        _mockOfferChecker = new();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Handle_ForValidGetStatusQuery_ShouldReturnBaseMessageDtoWithObjectTrue(bool offerStatus)
    {
        //Arrange
        GetStatusQuery getStatusQuery = new()
        {
            OfferId = Guid.NewGuid()
        };
        Offer offer = new(
            "Title test",
            "Test postition type",
            10000,
            15000,
            "Description test Description test Description test",
            Guid.NewGuid(),
            offerStatus
        );
        _mockOfferRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(offer);
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(true);
        IValidator<GetStatusQuery> validator = new GetStatusValidator(_mockOfferChecker.Object);
        var getStatusQueryHandler = new GetStatusQueryHandler(_mockLogger.Object, _mockUnitOfWork.Object, validator);
        //Act
        var result = await getStatusQueryHandler.Handle(getStatusQuery, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Errors.Should().BeNull();
        result.Object.Should().Be(offerStatus);
    }
}