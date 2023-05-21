using Application.CQRS.Offers.Commands.DeleteOffer;
using Domain.Interfaces.Validation;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;

namespace Application.Tests.CQRS.Offers.Commands.DeleteOffer;

public class DeleteOfferCommandValidatorTests
{
    private readonly Mock<IOfferChecker> _mockOfferChecker;
    private readonly IValidator<DeleteOfferCommand> _validator;
    public DeleteOfferCommandValidatorTests()
    {
        _mockOfferChecker = new();
        _validator = new DeleteOfferCommandValidator(_mockOfferChecker.Object);
    }

    [Theory]
    [ClassData(typeof(DeleteOfferCommandValidDataProvider))]
    public async Task DeleteOfferCommandValidator_ForValidDeleteCommandAndExistedOffer_ShouldNotHaveAnyValidationErrors(DeleteOfferCommand deleteOfferCommand)
    {
        //Arrange
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(true);
        //Act
        var result = await _validator.TestValidateAsync(deleteOfferCommand);
        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    
    [Theory]
    [ClassData(typeof(DeleteOfferCommandInvalidDataProvider))]
    public async Task DeleteOfferCommandValidator_ForInValidDeleteCommandAndExistedOffer_ShouldNotHaveAnyValidationErrors(DeleteOfferCommand deleteOfferCommand)
    {
        //Arrange
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(true);
        //Act
        var result = await _validator.TestValidateAsync(deleteOfferCommand);
        //Assert
        result.ShouldHaveAnyValidationError();
    }
    
    [Theory]
    [ClassData(typeof(DeleteOfferCommandValidDataProvider))]
    public async Task DeleteOfferCommandValidator_ForValidDeleteCommandAndNotExistedOffer_ShouldNotHaveAnyValidationErrors(DeleteOfferCommand deleteOfferCommand)
    {
        //Arrange
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(false);
        //Act
        var result = await _validator.TestValidateAsync(deleteOfferCommand);
        //Assert
        result.ShouldHaveAnyValidationError();
    }
}