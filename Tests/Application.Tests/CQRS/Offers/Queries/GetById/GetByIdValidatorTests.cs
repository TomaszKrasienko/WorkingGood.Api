using System;
using Application.CQRS.Offers.Queries.GetById;
using Application.CQRS.Offers.Queries.GetOfferStatus;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Application.Tests.CQRS.Offers.Queries.GetById
{
	public class GetByIdValidatorTests
	{
		[Fact]
		public async Task GetByIdValidator_ForValidGetByIdQuery_ShouldHaveNotErrors()
		{
            //Arrange
            GetByIdQuery getByIdQuery = new()
            {
                Id = Guid.NewGuid()
			};
            IValidator<GetByIdQuery> validator = new GetByIdValidator();
			//Act
			var result = await validator.TestValidateAsync(getByIdQuery);
			//Assert
			result.ShouldNotHaveAnyValidationErrors();
        }
        [Theory]
        [ClassData(typeof(GetByIdTestInvalidDataProvider))]
        public async Task GetByIdValidator_ForInvalidGetByIdQuery_ShouldHaveErrors(GetByIdQuery getByIdQuery)
        {
            //Arrange
            IValidator<GetByIdQuery> validator = new GetByIdValidator();
            //Act
            var result = await validator.TestValidateAsync(getByIdQuery);
            //Assert
            result.ShouldHaveAnyValidationError();
        }
    }
}

