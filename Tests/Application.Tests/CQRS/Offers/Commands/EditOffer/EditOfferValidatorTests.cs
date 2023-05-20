using Application.CQRS.Offers.Commands.EditOffer;
using Domain.Interfaces.Validation;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;

namespace Application.Tests.CQRS.Offers.Commands.EditOffer;

public class EditOfferValidatorTests
{
    private readonly IValidator<EditOfferCommand> _validator;
    private readonly Mock<IOfferChecker> _mockOfferChecker;
    public EditOfferValidatorTests()
    {
        _mockOfferChecker = new();
        _validator = new EditOfferValidator(_mockOfferChecker.Object);
    }
    [Fact]
    public async Task EditOfferValidator_ForValidEditOfferCommandAndExistedOffer_ShouldHaveNotErrors()
    {
        //Arrange
        EditOfferCommand addOfferCommand = new()
        {
            OfferId = Guid.NewGuid(),
            OfferDto = new()
            {
                Title = "Title",
                Description = "Description description Description description",
                PositionType = "Test",
                SalaryRangeMax = 12000,
                SalaryRangeMin = 10000,
                IsActive = true
            }
        };
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(true);
        //Act
        var result = await _validator.TestValidateAsync(addOfferCommand);
        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    [Theory]
    [ClassData(typeof(EditOfferTestInvalidDataProvider))]
    public async Task EditOfferValidator_ForInvalidEditOfferCommandAndExistedOffer_ShouldHaveErrors(EditOfferCommand editOfferCommand)
    {
        //Arrange
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(true);
        //Act
        var result = await _validator.TestValidateAsync(editOfferCommand);
        //Assert
        result.ShouldHaveAnyValidationError();
    }

    [Fact]
    public async Task EditOfferValidator_ForValidEditOfferCommandAndNonExistedOffer_ShouldHaveErrors()
    {
        //Arrange
        EditOfferCommand addOfferCommand = new()
        {
            OfferId = Guid.NewGuid(),
            OfferDto = new()
            {
                Title = "Title",
                Description = "Description description Description description",
                PositionType = "Test",
                SalaryRangeMax = 12000,
                SalaryRangeMin = 10000,
                IsActive = true
            }
        };
        _mockOfferChecker
            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(false);
        //Act
        var result = await _validator.TestValidateAsync(addOfferCommand);
        //Assert
        result.ShouldHaveAnyValidationError();
    }
}