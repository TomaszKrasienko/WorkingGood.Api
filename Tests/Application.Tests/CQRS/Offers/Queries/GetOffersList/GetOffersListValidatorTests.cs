using Application.CQRS.Offers.Queries.GetOffersList;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Application.Tests.CQRS.Offers.Queries.GetOffersList;

public class GetOffersListValidatorTests
{
    private readonly IValidator<GetOffersListQuery> _validator;

    public GetOffersListValidatorTests()
    {
        _validator = new GetOffersListValidator();
    }
    [Theory]
    [ClassData(typeof(GetOffersListQueryValidDataProvider))]
    public async Task Validate_ForValidGetOffersListQuery_ShouldNotHaveAnyErrors(GetOffersListQuery getOffersListQuery)
    {
        //Act
        var result = await _validator.TestValidateAsync(getOffersListQuery);
        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    [Theory]
    [ClassData(typeof(GetOffersListQueryInValidDataProvider))]
    public async Task Validate_ForInValidGetOffersListQuery_ShouldHaveAnyErrors(GetOffersListQuery getOffersListQuery)
    {
        //Act
        var result = await _validator.TestValidateAsync(getOffersListQuery);
        //Assert
        result.ShouldHaveAnyValidationError();
    }
}