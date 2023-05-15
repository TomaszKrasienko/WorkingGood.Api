// using Application.CQRS.Offers.Queries.GetAllForCompany;
// using FluentValidation;
// using FluentValidation.TestHelper;
//
// namespace Application.Tests.CQRS.Offers.Queries.GetAllForCompany;
//
// public class GetAllForCompanyValidatorTests
// {
//        private IValidator<GetAllForCompanyQuery> _validator;
//
//        [Fact]
//        public async Task GetAllForCompanyValidator_ForValidGetAllForCompanyQuery_ShouldHaveNotAnyErrors()
//        { 
//               //Arrange
//               GetAllForCompanyQuery getAllForCompanyQuery = new()
//               {
//                      EmployeeId = Guid.NewGuid()
//               };
//               _validator = new GetAllForEmployeeValidator();
//               //Act
//               var result = await _validator.TestValidateAsync(getAllForCompanyQuery);
//               //Assert
//               result.ShouldNotHaveAnyValidationErrors();
//        }
//        [Theory]
//        [ClassData(typeof(GetAllForCompanyTestInvalidDataProvider))]
//        public async Task GetAllForCompanyValidator_ForInvalidGetAllForCompanyQuery_ShouldHaveNotAnyErrors(GetAllForCompanyQuery getAllForCompanyQuery)
//        { 
//               //Arrange
//               _validator = new GetAllForEmployeeValidator();
//               //Act
//               var result = await _validator.TestValidateAsync(getAllForCompanyQuery);
//               //Assert
//               result.ShouldHaveAnyValidationError();
//        }
// }