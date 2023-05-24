using Application.CQRS.Employees.Queries;
using FluentValidation;
using FluentValidation.TestHelper;

namespace Application.Tests.CQRS.Employees.Queries.GetEmployeeByIdQueryHandlerTests;

public class GetEmployeeValidatorTests
{
    private IValidator<GetEmployeeByIdQuery> _validator;

    [Fact]
    public async Task GetEmployeeByIdValidator_ForValidGetEmployeeGetEmployeeByIdQuery_ShouldHaveNotErrors()
    {
        //Arrange
        GetEmployeeByIdQuery getEmployeeByIdQuery = new()
        {
            Id = Guid.NewGuid()
        };
        _validator = new GetEmployeeByIdValidator();
        //Act
        var result = await _validator.TestValidateAsync(getEmployeeByIdQuery);
        //Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
    [Fact]
    public async Task GetEmployeeByIdValidator_ForInValidGetEmployeeGetEmployeeByIdQuery_ShouldHaveErrorsForId()
    {
        //Arrange
        GetEmployeeByIdQuery getEmployeeByIdQuery = new();
        _validator = new GetEmployeeByIdValidator();
        //Act
        var result = await _validator.TestValidateAsync(getEmployeeByIdQuery);
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }
}