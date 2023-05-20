using Application.CQRS.Offers.Commands.EditOffer;
using Application.DTOs;
using Application.ViewModels.Offer;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Validation;
using Domain.Models.Offer;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.CQRS.Offers.Commands.EditOffer;

public class EditOfferCommandHandlerTests
{
    private readonly Mock<ILogger<EditOfferCommandHandler>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IOfferRepository> _mockOfferRepository;
    private readonly Mock<IOfferChecker> _mockOfferChecker;
    private readonly Mock<IMapper> _mockMapper;
    public EditOfferCommandHandlerTests()
    {
        _mockLogger = new();
        _mockUnitOfWork = new();
        _mockOfferRepository = new();
        _mockOfferChecker = new();
        _mockUnitOfWork
            .Setup(x => x.OffersRepository)
            .Returns(_mockOfferRepository.Object);
        _mockMapper = new();
    }

    [Fact]
    public async Task Handle_ForValidEditOfferCommandAndExistedOffer_ShouldReturnBaseMessageWithGetOfferVMObjectAndMessage()
    {
        //Arrange
        EditOfferCommand editOfferCommand = new()
        {
            OfferId = Guid.NewGuid(),
            OfferDto = new()
            {
                Title = "newTestTitle",
                Description = "newTestDescriptionnewTestDescriptionnewTestDescriptionnewTestDescription",
                SalaryRangeMin = 10000,
                SalaryRangeMax = 12000,
                IsActive = true,
                PositionType = "newPositionType"
            }
        };
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(true);
        IValidator<EditOfferCommand> validator = new EditOfferValidator(_mockOfferChecker.Object);
        Offer offer = new(
            "Title test",
            "Test postition type",
            10000,
            15000,
            "Description test Description test Description test",
            Guid.NewGuid(),
            false
        );
        _mockOfferRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(offer);
        _mockMapper
            .Setup(x => x.Map<GetOfferVM>(It.IsAny<Offer>()))
            .Returns(new GetOfferVM()
            {
                Id = offer.Id,
                Title = editOfferCommand.OfferDto.Title,
                Description = editOfferCommand.OfferDto.Description,
                SalaryRangeMin = (double) editOfferCommand.OfferDto.SalaryRangeMin,
                SalaryRangeMax = (double) editOfferCommand.OfferDto.SalaryRangeMax,
                PositionType = editOfferCommand.OfferDto.PositionType,
                IsActive = (bool) editOfferCommand.OfferDto.IsActive,
            });
        EditOfferCommandHandler editOfferCommandHandler = new EditOfferCommandHandler(
            _mockLogger.Object,
            _mockUnitOfWork.Object,
            validator,
            _mockMapper.Object);
        //Act
        var result = await editOfferCommandHandler.Handle(editOfferCommand, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().Be("Offer edited successfully");
        result.Object.Should().BeOfType<GetOfferVM>();
    }
    [Fact]
    public async Task Handle_ForValidEditOfferCommandAndNonExistedOffer_ShouldReturnBaseMessageWithGetOfferVMObjectAndMessage()
    {
        //Arrange
        EditOfferCommand editOfferCommand = new()
        {
            OfferId = Guid.NewGuid(),
            OfferDto = new()
            {
                Title = "newTestTitle",
                Description = "newTestDescriptionnewTestDescriptionnewTestDescriptionnewTestDescription",
                SalaryRangeMin = 10000,
                SalaryRangeMax = 12000,
                IsActive = true,
                PositionType = "newPositionType"
            }
        };
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(false);
        IValidator<EditOfferCommand> validator = new EditOfferValidator(_mockOfferChecker.Object);
        EditOfferCommandHandler editOfferCommandHandler = new EditOfferCommandHandler(
            _mockLogger.Object,
            _mockUnitOfWork.Object,
            validator,
            _mockMapper.Object);
        //Act
        var result = await editOfferCommandHandler.Handle(editOfferCommand, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().BeNull();
        result.Object.Should().BeNull();
        result.Errors.Should().NotBeNull();
    }
    [Theory]
    [ClassData(typeof(EditOfferTestInvalidDataProvider))]
    public async Task Handle_ForInValidEditOfferCommandAndExistedOffer_ShouldReturnBaseMessageWithGetOfferVMObjectAndMessage(EditOfferCommand editOfferCommand)
    {
        //Arrange
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(false);
        IValidator<EditOfferCommand> validator = new EditOfferValidator(_mockOfferChecker.Object);
        EditOfferCommandHandler editOfferCommandHandler = new EditOfferCommandHandler(
            _mockLogger.Object,
            _mockUnitOfWork.Object,
            validator,
            _mockMapper.Object);
        //Act
        var result = await editOfferCommandHandler.Handle(editOfferCommand, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().BeNull();
        result.Object.Should().BeNull();
        result.Errors.Should().NotBeNull();
    }
}