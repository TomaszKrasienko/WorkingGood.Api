using Application.CQRS.Offers.Commands;
using Application.CQRS.Offers.Commands.AddOffer;
using Application.DTOs;
using Application.Tests.Helpers;
using Application.ViewModels.Offer;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models.Offer;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using NLog;
using WorkingGood.Log;

namespace Application.Tests.CQRS.Offers.Commands.AddOffer;

public class AddOfferCommandHandlerTests
{
    private readonly Mock<IWgLog<AddOfferCommandHandler>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IOfferRepository> _mockOfferRepository;
    private readonly Mock<IMapper> _mockMapper;
    public AddOfferCommandHandlerTests()
    {
        _mockLogger = new();
        _mockUnitOfWork = new();
        _mockOfferRepository = new();
        _mockUnitOfWork.Setup(x => x.OffersRepository)
            .Returns(_mockOfferRepository.Object);
        _mockMapper = new();
    }

    [Theory]
    [ClassData(typeof(AddOfferCommandValidDataProvider))]
    public async Task Handle_ForValidaAddOfferCommand_ShouldReturnBaseMessageDtoWithObjectAndMessage(AddOfferCommand addOfferCommand)
    {
        //Arrange
        IValidator<AddOfferCommand> validator = new AddOfferValidator();
        _mockMapper
            .Setup(x => x.Map<GetOfferVM>(It.IsAny<Offer>()))
            .Returns(ObjectProvider.GetGetOfferVm());
        AddOfferCommandHandler addOfferCommandHandler = new(_mockLogger.Object, _mockUnitOfWork.Object, validator, _mockMapper.Object);
        //Act
        var result = await addOfferCommandHandler.Handle(addOfferCommand, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().Be("Offer added successfully");
        result.Object.Should().BeOfType<GetOfferVM>();
    }

    [Theory]
    [ClassData(typeof(AddOfferCommandInvalidDataProvider))]
    public async Task Handle_ForInvalidAddOfferCommand_ShouldReturnBaseMessageDtoWithErrors(
        AddOfferCommand addOfferCommand)
    {
        //Arrange
        IValidator<AddOfferCommand> validator = new AddOfferValidator();
        AddOfferCommandHandler addOfferCommandHandler = new(_mockLogger.Object, _mockUnitOfWork.Object, validator, _mockMapper.Object);
        //Act
        var result = await addOfferCommandHandler.Handle(addOfferCommand, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().BeNull();
        result.Object.Should().BeNull();
        result.MetaData.Should().BeNull();
        result.Errors.Should().NotBeNull();
    }
}