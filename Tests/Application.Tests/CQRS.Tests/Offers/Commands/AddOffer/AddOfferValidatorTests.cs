using Application.CQRS.Offers.Commands;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Application.Tests.CQRS.Offers.Commands.AddOffer;

public class AddOfferValidatorTests
{
    private readonly IValidator<AddOfferCommand> _validator;
    public AddOfferValidatorTests()
    {
        _validator = new AddOfferValidator();
    }
    [Theory]
    [ClassData(typeof(AddOfferCommandValidDataProvider))]
    public async Task AddOfferValidator_ForValidAddOfferCommand_ShouldHaveNotErrors(AddOfferCommand addOfferCommand)
    {
        //Act
        var result = await _validator.TestValidateAsync(addOfferCommand);
        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    [Theory]
    [ClassData(typeof(AddOfferCommandInvalidDataProvider))]
    public async Task AddOfferValidator_ForInvalidAddOfferCommand_ShouldHaveErrors(AddOfferCommand addOfferCommand)
    {
        //Act
        var result = await _validator.TestValidateAsync(addOfferCommand);
        //Assert
        result.ShouldHaveAnyValidationError();
    }
}