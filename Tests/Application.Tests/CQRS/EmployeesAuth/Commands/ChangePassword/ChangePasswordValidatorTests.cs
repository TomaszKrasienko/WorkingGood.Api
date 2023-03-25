using Application.CQRS.EmployeesAuth.Commands.ChangePassword;
using Domain.Interfaces.Validation;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.ChangePassword;

public class ChangePasswordValidatorTests
{
    private IValidator<ChangePasswordCommand> _validator;
    [Fact]
    public async Task ChangePasswordValidator_ForValidNewPassword_ShouldHaveNotErrorsForNewPassword()
    {
        //Arrange
            ChangePasswordCommand changePasswordCommand = new()
            {
                EmployeeId = Guid.NewGuid(),
                ChangePasswordDto = new()
                {
                    NewPassword = "123Test!",
                    OldPassword = "",
                    ConfirmNewPassword = ""
                }
            };
            Mock<IEmployeeChecker> mockEmployeeChecker = new Mock<IEmployeeChecker>();
            _validator = new ChangePasswordValidator(mockEmployeeChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(changePasswordCommand);
        //Assert
            result.ShouldNotHaveValidationErrorFor(cpc => cpc.ChangePasswordDto.NewPassword);
    }
    [Theory]
    [InlineData("test")]
    [InlineData("TEST")]
    [InlineData("Test")]
    public async Task ChangePasswordValidator_ForInvalidNewPassword_ShouldHaveErrorsForPassword(string password)
    {        
        //Arrange
            ChangePasswordCommand changePasswordCommand = new()
            {
                EmployeeId = Guid.NewGuid(),
                ChangePasswordDto = new()
                {
                    NewPassword = password,
                    OldPassword = "",
                    ConfirmNewPassword = ""
                }
            };
            Mock<IEmployeeChecker> mockEmployeeChecker = new Mock<IEmployeeChecker>();
            _validator = new ChangePasswordValidator(mockEmployeeChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(changePasswordCommand);
        //Assert
            result.ShouldHaveValidationErrorFor(cpc => cpc.ChangePasswordDto.NewPassword);
    }
    [Fact]
    public async Task ChangePasswordValidator_ForValidOldPassword_ShouldHaveNotErrorsForOldPassword()
    {
        //Arrange
            ChangePasswordCommand changePasswordCommand = new()
            {
                EmployeeId = Guid.NewGuid(),
                ChangePasswordDto = new()
                {
                    NewPassword = "",
                    OldPassword = "123",
                    ConfirmNewPassword = ""
                }
            };
            Mock<IEmployeeChecker> mockEmployeeChecker = new Mock<IEmployeeChecker>();
            _validator = new ChangePasswordValidator(mockEmployeeChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(changePasswordCommand);
        //Assert
            result.ShouldNotHaveValidationErrorFor(cpc => cpc.ChangePasswordDto.OldPassword);
    }    
    [Fact]
    public async Task ChangePasswordValidator_ForInvalidOldPassword_ShouldHaveNotErrorsForOldPassword()
    {
        //Arrange
            ChangePasswordCommand changePasswordCommand = new()
            {
                EmployeeId = Guid.NewGuid(),
                ChangePasswordDto = new()
                {
                    NewPassword = "",
                    OldPassword = "",
                    ConfirmNewPassword = ""
                }
            };
            Mock<IEmployeeChecker> mockEmployeeChecker = new Mock<IEmployeeChecker>();
            _validator = new ChangePasswordValidator(mockEmployeeChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(changePasswordCommand);
        //Assert
            result.ShouldHaveValidationErrorFor(cpc => cpc.ChangePasswordDto.OldPassword);
    }
    [Fact]
    public async Task ChangePasswordValidator_ForValidConfirmNewPassword_ShouldHaveNotErrorsForConfirmNewPassword()
    {
        //Arrange
            ChangePasswordCommand changePasswordCommand = new()
            {
                EmployeeId = Guid.NewGuid(),
                ChangePasswordDto = new()
                {
                    NewPassword = "",
                    OldPassword = "",
                    ConfirmNewPassword = "123"
                }
            };
            Mock<IEmployeeChecker> mockEmployeeChecker = new Mock<IEmployeeChecker>();
            _validator = new ChangePasswordValidator(mockEmployeeChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(changePasswordCommand);
        //Assert
            result.ShouldNotHaveValidationErrorFor(cpc => cpc.ChangePasswordDto.ConfirmNewPassword);
    }    
    [Fact]
    public async Task ChangePasswordValidator_ForInvalidConfirmNewPassword_ShouldHaveNotErrorsConfirmNewPassword()
    {
        //Arrange
            ChangePasswordCommand changePasswordCommand = new()
            {
                EmployeeId = Guid.NewGuid(),
                ChangePasswordDto = new()
                {
                    NewPassword = "",
                    OldPassword = "",
                    ConfirmNewPassword = ""
                }
            };
            Mock<IEmployeeChecker> mockEmployeeChecker = new Mock<IEmployeeChecker>();
            _validator = new ChangePasswordValidator(mockEmployeeChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(changePasswordCommand);
        //Assert
            result.ShouldHaveValidationErrorFor(cpc => cpc.ChangePasswordDto.ConfirmNewPassword);
    }
    [Fact]
    public async Task ChangePasswordValidator_ForMatchingNewPasswords_ShouldHaveNotErrorsForMatching()
    {
        //Arrange
            string password = "Test123";
            ChangePasswordCommand changePasswordCommand = new()
            {
                EmployeeId = Guid.NewGuid(),
                ChangePasswordDto = new()
                {
                    NewPassword = password,
                    OldPassword = "",
                    ConfirmNewPassword = password
                }
            };
            Mock<IEmployeeChecker> mockEmployeeChecker = new Mock<IEmployeeChecker>();
            _validator = new ChangePasswordValidator(mockEmployeeChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(changePasswordCommand);
        //Assert
            var findedResult = result.Errors.FirstOrDefault(x => x.ErrorMessage == "Password not match to confirmation");
            findedResult.Should().BeNull();
    }    
    [Fact]
    public async Task ChangePasswordValidator_ForNonMatchingNewPasswords_ShouldHaveErrorsForMatching()
    {
        //Arramge
            string password1 = "Test123";
            string password2 = "123Test";
            ChangePasswordCommand changePasswordCommand = new()
            {
                EmployeeId = Guid.NewGuid(),
                ChangePasswordDto = new()
                {
                    NewPassword = password1,
                    OldPassword = "",
                    ConfirmNewPassword = password2
                }
            };
            Mock<IEmployeeChecker> mockEmployeeChecker = new Mock<IEmployeeChecker>();
            _validator = new ChangePasswordValidator(mockEmployeeChecker.Object);
        //Act
            var result = await _validator.TestValidateAsync(changePasswordCommand);
        //Assert
            var findedResult = result.Errors.FirstOrDefault(x => x.ErrorMessage == "Password not match to confirmation");
            findedResult.Should().NotBeNull();
    }
}