using System;
using Application.CQRS.Offers.Queries.GetById;
using Application.CQRS.Offers.Queries.GetOfferStatus;
using Domain.Interfaces.Validation;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;

namespace Application.Tests.CQRS.Offers.Queries.GetById
{
	public class GetActiveOfferByIdValidatorTests
	{
		private Mock<IOfferChecker> _mockOfferChecker;

		public GetActiveOfferByIdValidatorTests()
		{
			_mockOfferChecker = new();
		}
		[Fact]
		public async Task GetActiveOfferByIdValidator_ForValidGetByIdQuery_ShouldHaveNotErrors()
		{
            //Arrange
            GetActiveOfferByIdQuery getByIdQuery = new()
            {
                Id = Guid.NewGuid()
			};
            _mockOfferChecker
	            .Setup(x => x.IsOfferActive(It.IsAny<Guid>()))
	            .Returns(true);
            _mockOfferChecker
	            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
	            .Returns(true);
            IValidator<GetActiveOfferByIdQuery> validator = new GetActiveOfferByIdValidator(_mockOfferChecker.Object);
			//Act
			var result = await validator.TestValidateAsync(getByIdQuery);
			//Assert
			result.ShouldNotHaveAnyValidationErrors();
        }
        [Theory]
        [ClassData(typeof(GetActiveOfferByIdInvalidDataProvider))]
        public async Task GetActiveOfferByIdValidator_ForInvalidGetByIdQuery_ShouldHaveErrors(GetActiveOfferByIdQuery getByIdQuery)
        {
            //Arrange
            _mockOfferChecker
	            .Setup(x => x.IsOfferActive(It.IsAny<Guid>()))
	            .Returns(true);
            _mockOfferChecker
	            .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
	            .Returns(true);
            IValidator<GetActiveOfferByIdQuery> validator = new GetActiveOfferByIdValidator(_mockOfferChecker.Object);
            //Act
            var result = await validator.TestValidateAsync(getByIdQuery);
            //Assert
            result.ShouldHaveAnyValidationError();
        }
        [Fact]
        public async Task GetActiveOfferByIdValidator_ForNonExistsOffer_ShouldHaveErrors()
        {
	        //Arrange
	        GetActiveOfferByIdQuery getByIdQuery = new()
	        {
		        Id = Guid.NewGuid()
	        };
	        _mockOfferChecker
		        .Setup(x => x.IsOfferActive(It.IsAny<Guid>()))
		        .Returns(true);
	        _mockOfferChecker
		        .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
		        .Returns(false);
	        IValidator<GetActiveOfferByIdQuery> validator = new GetActiveOfferByIdValidator(_mockOfferChecker.Object);
	        //Act
	        var result = await validator.TestValidateAsync(getByIdQuery);
	        //Assert
	        result.ShouldHaveAnyValidationError();
        }
        [Fact]
        public async Task GetActiveOfferByIdValidator_ForNonActiveOffer_ShouldHaveErrors()
        {
	        //Arrange
	        GetActiveOfferByIdQuery getByIdQuery = new()
	        {
		        Id = Guid.NewGuid()
	        };
	        _mockOfferChecker
		        .Setup(x => x.IsOfferActive(It.IsAny<Guid>()))
		        .Returns(true);
	        _mockOfferChecker
		        .Setup(x => x.IsOfferExists(It.IsAny<Guid>()))
		        .Returns(false);
	        IValidator<GetActiveOfferByIdQuery> validator = new GetActiveOfferByIdValidator(_mockOfferChecker.Object);
	        //Act
	        var result = await validator.TestValidateAsync(getByIdQuery);
	        //Assert
	        result.ShouldHaveAnyValidationError();
        }
    }
}

