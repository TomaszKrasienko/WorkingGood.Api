using Domain.Common.Exceptions;
using Domain.Models.Employee;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.Models.Tests;

public class EmployeeTests
{
    [Fact]
    public void Activate_Always_ShouldChangeIsActiveAndShouldAddConfirmDate()
    {
        //Arrange
            Employee employee = new Employee("test", "test", "test", "test", Guid.NewGuid());
        //Act
            employee.Activate();
        //Assert
            employee.IsActive.Should().BeTrue();
            employee.VerificationToken.ConfirmDate.Should().NotBeNull();
    }

    [Fact]
    public void Login_ForActiveUserAndCorrectPassword_ShouldReturnLoginToken()
    {
        //Arrange
            string password = "testPassword";
            Employee employee = new Employee("test", "test", "test", password, Guid.NewGuid());
            employee.Activate();
        //Act
            LoginToken loginToken = employee.Login(password, "my top secret key", "test", "test");
        //Assert
            loginToken.Token.Should().NotBeNullOrEmpty();
    }
    [Fact]
    public void Login_ForNonActiveUserAndCorrectPassword_ShouldThrowLoginException()
    {
        //Arrange
            string password = "testPassword";
            Employee employee = new Employee("test", "test", "test", password, Guid.NewGuid());
        //Act
            Action login = () =>  employee.Login(password, "my top secret key", "test", "test");
        //Assert
            login.Should().ThrowExactly<LoginException>();
    }
    [Fact]
    public void Login_ForActiveUserAndCorrectPassword_ShouldThrowLoginException()
    {
        //Arrange
            string password = "testPassword";
            string wrongPassword = "wrongPassword";
            Employee employee = new Employee("test", "test", "test", password, Guid.NewGuid());
            employee.Activate();
        //Act
            Action login = () =>  employee.Login(wrongPassword, "my top secret key", "test", "test");
        //Assert
            login.Should().ThrowExactly<LoginException>();
    }
    [Fact]
    public void IsPasswordMatch_ForMatchingPassword_ShouldReturnTrue()
    {
        //Arrange
            string password = "testPassword";
            Employee employee = new Employee("test", "test", "test", password, Guid.NewGuid());
        //Act
            bool isPasswordMatching = employee.IsPasswordMatch(password);
        //Assert
            isPasswordMatching.Should().BeTrue();
    }
    [Fact]
    public void IsPasswordMatch_ForNonMatchingPassword_ShouldReturnFalse()
    {
        //Arrange
            string password = "testPassword";
            string wrongPassword = "wrongPassword";
            Employee employee = new Employee("test", "test", "test", password, Guid.NewGuid());
        //Act
            bool isPasswordMatching = employee.IsPasswordMatch(wrongPassword);
        //Assert
            isPasswordMatching.Should().BeFalse();
    }
    [Fact]
    public void Refresh_AfterLogin_ShouldSetNewRefreshTokenAndReturnNewLoginToken()
    {
        //Arrange
            string password = "testPassword";
            string tokenKey = "my top secret key";
            string audience = "test";
            string issuer = "test";
            Employee employee = new Employee("test", "test", "test", password, Guid.NewGuid());
            employee.Activate();
            LoginToken loginToken = employee.Login(password, tokenKey, audience, issuer);
            string currentRefreshToken = employee.RefreshToken.Token!;
        //Act
            LoginToken newLoginToken = employee.Refresh(tokenKey, audience, issuer);
        //Assert
            employee.RefreshToken.Token.Should().NotBeEquivalentTo(currentRefreshToken);
            newLoginToken.Should().NotBeEquivalentTo(loginToken);
    }
    [Fact]
    public void SetNewPassword_forStringPassword_ShouldChangePassword()
    {
        //Arrange
            string password = "testPassword";
            string newPassword = "testPassword";
            Employee employee = new Employee("test", "test", "test", password, Guid.NewGuid());
            byte[] oldPasswordSalt = employee.Password.Salt;
            byte[] oldPasswordHash = employee.Password.Hash;
        //Act
            employee.SetNewPassword(newPassword);
        //Assert
            oldPasswordHash.Should().NotBeEquivalentTo(employee.Password.Hash);
            oldPasswordSalt.Should().NotBeEquivalentTo(employee.Password.Salt);
    }
    [Fact]
    public void SetResetToken_ForEmployee_ShouldSetNewResetToken()
    {
        //Arrange
            string password = "testPassword";
            Employee employee = new Employee("test", "test", "test", password, Guid.NewGuid());
            string? oldResetToken = employee.ResetToken?.Token;
        //Act
            employee.SetResetToken();
        //Assert
            employee.ResetToken!.Token.Should().NotBeNull();
            employee.ResetToken!.ExpirationDate.Should().NotBeNull();
            employee.ResetToken!.Token.Should().NotBeEquivalentTo(oldResetToken);
    }
}