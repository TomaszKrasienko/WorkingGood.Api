using Application.CQRS.Offers.Commands.ChangeOfferStatus;
using Application.DTOs;
using Application.Tests.Helpers;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Validation;
using Domain.Models.Offer;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.CQRS.Offers.Commands.ChangeOfferStatus;

public class ChangeStatusCommandHandlerTests
{
    private readonly Mock<ILogger<ChangeOfferStatusCommandHandler>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IOfferRepository> _mockOfferRepository;
    private readonly Mock<IOfferChecker> _mockOfferChecker;

    public ChangeStatusCommandHandlerTests()
    {
        _mockLogger = new();
        _mockUnitOfWork = new();
        _mockOfferRepository = new();
        _mockOfferChecker = new();
        _mockUnitOfWork
            .Setup(x => x.OffersRepository)
            .Returns(_mockOfferRepository.Object);
    }
    
    [Theory]
    [ClassData(typeof(ChangeOfferStatusCommandValidDataProvider))]
    public async Task Handle_ForValidChangeOfferStatusCommandAndExistedOffer_ShouldReturnBaseResponseWithMessage(ChangeOfferStatusCommand changeOfferStatusCommand)
    {
        //Arrange
        Offer offer = ObjectProvider.GetOffer();
        _mockOfferRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(offer);
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(true);
        IValidator<ChangeOfferStatusCommand>
            validator = new ChangeOfferStatusCommandValidator(_mockOfferChecker.Object);
        ChangeOfferStatusCommandHandler changeOfferStatusCommandHandler = new(
            _mockLogger.Object,
            _mockUnitOfWork.Object,
            validator);
        //Act
        var result = await changeOfferStatusCommandHandler.Handle(changeOfferStatusCommand, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().Be("Status has been changed");
        result.Object.Should().BeNull();
        result.Errors.Should().BeNull();
        result.MetaData.Should().BeNull();
    }

    [Theory]
    [ClassData(typeof(ChangeOfferStatusCommandInvalidDataProvider))]
    public async Task Handle_ForInvalidChangeOfferCommandStatusAndExistedOffer_ShouldReturnBaseResponseWithErrors(
        ChangeOfferStatusCommand changeOfferStatusCommand)
    {
        //Arrange
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(true);
        IValidator<ChangeOfferStatusCommand>
            validator = new ChangeOfferStatusCommandValidator(_mockOfferChecker.Object);
        ChangeOfferStatusCommandHandler changeOfferStatusCommandHandler = new ChangeOfferStatusCommandHandler(
            _mockLogger.Object,
            _mockUnitOfWork.Object,
            validator);
        //Act
        var result = await changeOfferStatusCommandHandler.Handle(changeOfferStatusCommand, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().BeNull();
        result.Object.Should().BeNull();
        result.Errors.Should().NotBeNull();
        result.MetaData.Should().BeNull();
    }
    [Theory]
    [ClassData(typeof(ChangeOfferStatusCommandValidDataProvider))]
    public async Task Handle_ForValidChangeOfferCommandStatusAndNotExistedOffer_ShouldReturnBaseResponseWithErrors(
        ChangeOfferStatusCommand changeOfferStatusCommand)
    {
        //Arrange
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(false);
        IValidator<ChangeOfferStatusCommand>
            validator = new ChangeOfferStatusCommandValidator(_mockOfferChecker.Object);
        ChangeOfferStatusCommandHandler changeOfferStatusCommandHandler = new ChangeOfferStatusCommandHandler(
            _mockLogger.Object,
            _mockUnitOfWork.Object,
            validator);
        //Act
        var result = await changeOfferStatusCommandHandler.Handle(changeOfferStatusCommand, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().BeNull();
        result.Object.Should().BeNull();
        result.Errors.Should().NotBeNull();
        result.MetaData.Should().BeNull();
    }
}