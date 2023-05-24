using Application.CQRS.Offers.Queries.GetOfferStatus;
using Application.CQRS.Offers.Queries.GetStatus;
using Domain.Interfaces.Validation;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;

namespace Application.Tests.CQRS.Offers.Queries.GetStatus;

public class GetStatusValidatorTests
{
    private readonly Mock<IOfferChecker> _mockOfferChecker;
    private IValidator<GetStatusQuery>? _validator;
    public GetStatusValidatorTests()
    {
        _mockOfferChecker = new();
    }
    [Fact]
    public async Task GetStatusValidator_ForValidGetStatusQuery_ShouldHaveNotErrors()
    {
        //Arrange
        GetStatusQuery statusQuery = new GetStatusQuery()
        {
            OfferId = Guid.NewGuid()
        };
        _mockOfferChecker.Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(true);
        _validator = new GetStatusValidator(_mockOfferChecker.Object);
        //Act
        var result = await _validator.TestValidateAsync(statusQuery);
        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    [Fact]
    public async Task GetStatusValidator_ForNotExistingOffer_ShouldHaveErrorForOfferId()
    {
        //Arrange
        GetStatusQuery statusQuery = new GetStatusQuery()
        {
            OfferId = Guid.NewGuid()
        };
        _mockOfferChecker.Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(false);
        _validator = new GetStatusValidator(_mockOfferChecker.Object);
        //Act
        var result = await _validator.TestValidateAsync(statusQuery);
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.OfferId);
    }
    [Theory]
    [ClassData(typeof(GetStatusTestInvalidDataProvider))]
    public async Task GetStatusValidator_ForInvalidGetStatusQuery_ShouldHaveError(GetStatusQuery getStatusQuery)
    {
        //Arrange
        _mockOfferChecker.Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
            .Returns(true);
        _validator = new GetStatusValidator(_mockOfferChecker.Object);
        //Act
        var result = await _validator.TestValidateAsync(getStatusQuery);
        //Assert
        result.ShouldHaveAnyValidationError();
    }
}