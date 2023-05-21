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

    [Fact]
    public async Task DeleteOfferCommandValidator_ForValidDeleteCommandAndExistedOffer_ShouldNotHaveAnyValidationErrors()
    {
        //Arrange
        DeleteOfferCommand deleteOfferCommand = new()
        {
            OfferId = Guid.NewGuid()
        };
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(true);
        //Act
        var result = await _validator.TestValidateAsync(deleteOfferCommand);
        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}