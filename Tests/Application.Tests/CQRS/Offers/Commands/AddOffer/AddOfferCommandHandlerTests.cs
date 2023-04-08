using Application.CQRS.Offers.Commands;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Models.Offer;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using NLog;

namespace Application.Tests.CQRS.Offers.Commands.AddOffer;

public class AddOfferCommandHandlerTests
{
    private readonly Mock<ILogger<AddOfferCommandHandler>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IOfferRepository> _mockOfferRepository;
    public AddOfferCommandHandlerTests()
    {
        _mockLogger = new();
        _mockUnitOfWork = new();
        _mockOfferRepository = new();
        _mockUnitOfWork.Setup(x => x.OffersRepository)
            .Returns(_mockOfferRepository.Object);
    }

    [Fact]
    public async Task Handle_ForValidaAddOfferCommand_ShouldReturnBaseMessageDtoWithObjectAndMessage()
    {
        //Arrange
        IValidator<AddOfferCommand> validator = new AddOfferValidator();
        AddOfferCommandHandler addOfferCommandHandler = new(_mockLogger.Object, _mockUnitOfWork.Object, validator);
        AddOfferCommand addOfferCommand = new()
        {
            EmployeeId = Guid.NewGuid(),
            OfferDto = new()
            {
                Title = "Title",
                Description = "Description description Description description",
                PositionType = "Test",
                SalaryRangeMax = 12000,
                SalaryRangeMin = 10000
            }
        };
        //Act
        var result = await addOfferCommandHandler.Handle(addOfferCommand, new CancellationToken());
        //Assert
        result.Should().BeOfType<BaseMessageDto>();
        result.Message.Should().Be("Offer added successfully");
        result.Object.Should().BeOfType<Offer>();

    }
}