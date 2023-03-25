using Application.CQRS.Companies.Commands;
using Domain.Interfaces.Validation;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;

namespace Application.Tests.CQRS.Companies.Commands;

public class AddCompanyValidatorTests
{
    private IValidator<AddCompanyCommand> _validator;
    [Fact]
    public async Task AddCompanyValidator_ForNotEmptyCompanyName_ShouldHaveNotErrorsForCompany()
    {
        //Arrange
        AddCompanyCommand addCompanyCommand = new()
        {
            CompanyDto = new()
            {
                Name = "TestName"
            }
        };
        Mock<ICompanyChecker> mockCompanyChecker = new Mock<ICompanyChecker>();
        mockCompanyChecker.Setup(x => x.IsCompanyExists(It.IsAny<string>()))
            .Returns(false);
        _validator = new AddCompanyValidator(mockCompanyChecker.Object);
        //Act
        var result = await _validator.TestValidateAsync(addCompanyCommand);
        //Assert
        result.ShouldNotHaveValidationErrorFor(acc => acc.CompanyDto.Name);
    }    
    [Fact]
    public async Task AddCompanyValidator_ForEmptyCompanyName_ShouldHaveErrorsForCompany()
    {
        //Arrange
        AddCompanyCommand addCompanyCommand = new()
        {
            CompanyDto = new()
            {
                Name = ""
            }
        };
        Mock<ICompanyChecker> mockCompanyChecker = new Mock<ICompanyChecker>();
        mockCompanyChecker.Setup(x => x.IsCompanyExists(It.IsAny<string>()))
            .Returns(false);
        _validator = new AddCompanyValidator(mockCompanyChecker.Object);
        //Act
        var result = await _validator.TestValidateAsync(addCompanyCommand);
        //Assert
        result.ShouldHaveValidationErrorFor(acc => acc.CompanyDto.Name);
    }    
    [Fact]
    public async Task AddCompanyValidator_ForExistingCompanyName_ShouldHaveErrorsForCompany()
    {
        //Arrange
        AddCompanyCommand addCompanyCommand = new()
        {
            CompanyDto = new()
            {
                Name = "CompanyName"
            }
        };
        Mock<ICompanyChecker> mockCompanyChecker = new Mock<ICompanyChecker>();
        mockCompanyChecker.Setup(x => x.IsCompanyExists(It.IsAny<string>()))
            .Returns(true);
        _validator = new AddCompanyValidator(mockCompanyChecker.Object);
        //Act
        var result = await _validator.TestValidateAsync(addCompanyCommand);
        //Assert
        result.ShouldHaveValidationErrorFor(acc => acc.CompanyDto.Name);
    }
}