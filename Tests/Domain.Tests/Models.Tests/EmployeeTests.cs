using Domain.Common.Exceptions;
using Domain.Models.Employee;
using Domain.Services;
using Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace Domain.Tests.Models.Tests;

public class EmployeeTests
{
    private readonly Mock<ITokenProvider> _mockTokenProvider;
    public EmployeeTests()
    {
        _mockTokenProvider = new();
    }
    [Fact]
    public void Activate_Always_ShouldChangeIsActiveAndShouldAddConfirmDate()
    {
        //Arrange
            Employee employee = new Employee("test", "test", "test", "test", Guid.NewGuid());
        //Act
            employee.Activate();
        //Assert
            employee.EmployeeStatus.IsActive.Should().BeTrue();
            employee.VerificationToken.ConfirmDate.Should().NotBeNull();
    }
    [Fact]
    public void Login_ForActiveUserAndCorrectPassword_ShouldReturnLoginToken()
    {
        //Arrange
            string password = "testPassword";
            Employee employee = new Employee("test", "test", "test", password, Guid.NewGuid());
            employee
                .Activate();
            _mockTokenProvider
                .Setup(x => x.Provide(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(new LoginToken()
                {
                    Expiration = DateTime.Now,
                    Token = "test string token"
                });
        //Act
            LoginToken loginToken = employee
                .Login(password, _mockTokenProvider.Object);
        //Assert
            loginToken.Token.Should().NotBeNullOrEmpty();
    }
    [Fact]
    public void Login_ForNonActiveUserAndCorrectPassword_ShouldThrowLoginException()
    {
        //Arrange
            string password = "testPassword";
            Employee employee = new Employee("test", "test", "test", password, Guid.NewGuid());
            _mockTokenProvider
                .Setup(x => x.Provide(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(new LoginToken()
                {
                    Expiration = DateTime.Now,
                    Token = "test string token"
                });
        //Act
            Action login = () =>  employee.Login(password, _mockTokenProvider.Object);
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
            _mockTokenProvider
                .Setup(x => x.Provide(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(new LoginToken()
                {
                    Expiration = DateTime.Now,
                    Token = "test string token"
                });
        //Act
            Action login = () =>  employee.Login(wrongPassword, _mockTokenProvider.Object);
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
            _mockTokenProvider
                .Setup(x => x.Provide(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(new LoginToken()
                {
                    Expiration = DateTime.Now,
                    Token = "test string token"
                });
            Employee employee = new Employee("test", "test", "test", password, Guid.NewGuid());
            employee.Activate();
            LoginToken loginToken = employee.Login(password, _mockTokenProvider.Object);
            string currentRefreshToken = employee.RefreshToken.Token!;
        //Act
            LoginToken newLoginToken = employee.Refresh(_mockTokenProvider.Object);
        //Assert
            employee.RefreshToken.Token.Should().NotBeEquivalentTo(currentRefreshToken);
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