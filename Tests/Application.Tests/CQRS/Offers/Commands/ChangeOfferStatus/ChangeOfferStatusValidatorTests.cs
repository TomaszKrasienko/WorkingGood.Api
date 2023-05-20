using Application.CQRS.Offers.Commands.ChangeOfferStatus;
using Domain.Interfaces.Validation;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;

namespace Application.Tests.CQRS.Offers.Commands.ChangeOfferStatus;

public class ChangeOfferStatusValidatorTests
{
    private readonly IValidator<ChangeOfferStatusCommand> _validator;
    private readonly Mock<IOfferChecker> _mockOfferChecker;

    public ChangeOfferStatusValidatorTests()
    {
        _mockOfferChecker = new();
        _validator = new ChangeOfferStatusCommandValidator(_mockOfferChecker.Object);
    }
    
    [Fact]
    public async Task ChangeOfferStatusValidation_ForValidChangeStatusCommandAndExistedOffer_ShouldNotHaveAnyError()
    {
        //Arrange
        ChangeOfferStatusCommand changeOfferStatusCommand = new()
        {
            OfferId = Guid.NewGuid()
        };
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(true);
        //Act
        var result = await _validator.TestValidateAsync(changeOfferStatusCommand);
        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task ChangeOfferStatusValidation_ForInvalidChangeStatusCommandAndNonExistedOffer_ShouldHaveAnyValidationErrors()
    {
        //Arrange
        ChangeOfferStatusCommand changeOfferStatusCommand = new()
        {
            OfferId = Guid.NewGuid()
        };
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(true);
        //Act
        var result = await _validator.TestValidateAsync(changeOfferStatusCommand);
        //Assert
        result.ShouldHaveAnyValidationError();
    }
    
    [Fact]
    public async Task ChangeOfferStatusValidation_ForValidChangeStatusCommandAndNonExistedOffer_ShouldNotHaveAnyError()
    {
        //Arrange
        ChangeOfferStatusCommand changeOfferStatusCommand = new()
        {
            OfferId = Guid.NewGuid()
        };
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(false);
        //Act
        var result = await _validator.TestValidateAsync(changeOfferStatusCommand);
        //Assert
        result.ShouldHaveAnyValidationError();
    }

}