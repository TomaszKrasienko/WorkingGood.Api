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
}