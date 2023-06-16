using System.Globalization;
using Application.CQRS.Offers.Commands.DeleteOffer;
using Application.DTOs;
using Application.Tests.Helpers;
using Domain.Interfaces;
using Domain.Interfaces.Communication;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Validation;
using Domain.Models.Offer;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using WorkingGood.Log;

namespace Application.Tests.CQRS.Offers.Commands.DeleteOffer;

public class DeleteOfferCommandHandlerTests
{
    private readonly Mock<IWgLog<DeleteOfferCommandHandler>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IOfferRepository> _mockOfferRepository;
    private readonly Mock<IOfferChecker> _mockOfferChecker;
    private readonly Mock<IBrokerSender> _mockBrokerSender;
    private readonly IValidator<DeleteOfferCommand> _validator;

    public DeleteOfferCommandHandlerTests()
    {
        _mockLogger = new();
        _mockUnitOfWork = new();
        _mockOfferRepository = new();
        _mockUnitOfWork
            .Setup(x => x.OffersRepository)
            .Returns(_mockOfferRepository.Object);
        _mockBrokerSender = new();
        _mockOfferChecker = new();
        _validator = new DeleteOfferCommandValidator(_mockOfferChecker.Object);
    }

    [Theory]
    [ClassData(typeof(DeleteOfferCommandValidDataProvider))]
    public async Task Handle_ForValidDeleteOfferCommandAndExistedOffer_ShouldReturnBaseMessageWithMessage(DeleteOfferCommand deleteOfferCommand)
    {
        //Arrange
        Offer offer = ObjectProvider.GetOffer();
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(true);
        _mockOfferRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(offer);
        DeleteOfferCommandHandler deleteOfferCommandHandler = new(
            _mockLogger.Object,
            _mockUnitOfWork.Object,
            _validator,
            _mockBrokerSender.Object);
        //Act
        var result = await deleteOfferCommandHandler.Handle(deleteOfferCommand, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().Be("Offer has been deleted");
        result.Errors.Should().BeNull();
        result.Object.Should().BeNull();
        result.MetaData.Should().BeNull();
    }

    [Theory]
    [ClassData(typeof(DeleteOfferCommandInvalidDataProvider))]
    public async Task Handle_ForInvalidDeleteOfferCommandAndExistedOffer_ShouldReturnBaseMessageWithErrors(
        DeleteOfferCommand deleteOfferCommand)
    {
        //Arrange
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(true);
        DeleteOfferCommandHandler deleteOfferCommandHandler = new(
            _mockLogger.Object,
            _mockUnitOfWork.Object,
            _validator,
            _mockBrokerSender.Object);
        //Act
        var result = await deleteOfferCommandHandler.Handle(deleteOfferCommand, new CancellationToken());
        //Asserts
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().BeNull();
        result.Object.Should().BeNull();
        result.MetaData.Should().BeNull();
        result.Errors.Should().NotBeNull();
    }
    [Theory]
    [ClassData(typeof(DeleteOfferCommandValidDataProvider))]
    public async Task Handle_ForValidDeleteOfferCommandAndNotExistedOffer_ShouldReturnBaseMessageWithErrors(
        DeleteOfferCommand deleteOfferCommand)
    {
        //Arrange
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(false);
        DeleteOfferCommandHandler deleteOfferCommandHandler = new(
            _mockLogger.Object,
            _mockUnitOfWork.Object,
            _validator,
            _mockBrokerSender.Object);
        //Act
        var result = await deleteOfferCommandHandler.Handle(deleteOfferCommand, new CancellationToken());
        //Asserts
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().BeNull();
        result.Object.Should().BeNull();
        result.MetaData.Should().BeNull();
        result.Errors.Should().NotBeNull();
    }
}