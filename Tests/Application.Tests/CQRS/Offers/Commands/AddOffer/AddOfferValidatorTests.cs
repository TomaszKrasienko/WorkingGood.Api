using Application.CQRS.Offers.Commands;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Application.Tests.CQRS.Offers.Commands.AddOffer;

public class AddOfferValidatorTests
{
    private IValidator<AddOfferCommand> _validator;

    [Fact]
    public async Task AddOfferValidator_ForValidAddOfferCommand_ShouldHaveNotErrors()
    {
        //Arrange
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
        _validator = new AddOfferValidator();
        //Act
        var result = await _validator.TestValidateAsync(addOfferCommand);
        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    [Theory]
    [ClassData(typeof(AddOfferTestInvalidDataProvider))]
    public async Task AddOfferValidator_ForInvalidAddOfferCommand_ShouldHaveErrors(AddOfferCommand addOfferCommand)
    {
        //Arrange
        _validator = new AddOfferValidator();
        //Act
        var result = await _validator.TestValidateAsync(addOfferCommand);
        //Assert
        result.ShouldHaveAnyValidationError();
    }
}