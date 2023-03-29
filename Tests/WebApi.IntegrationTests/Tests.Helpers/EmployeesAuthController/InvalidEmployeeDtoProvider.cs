using System.Collections;
using Application.DTOs.EmployeesAuth;
using Application.EmployeesAuth.Commands;

namespace WebApi.IntegrationTests.Tests.Helpers.EmployeesAuthController;

public class InvalidRegisterEmployeeDtoProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new RegisterEmployeeDto()
            {
                Email = "testpl", 
                Password = "Test123", 
                FirstName = "Test", 
                LastName = "Test"
            }
        };
        yield return new object[]
        {
            new RegisterEmployeeDto()
            {
                Email = "test@test.pl",
                Password = "test123",
                FirstName = "Test",
                LastName = "Test"
            }
        };
        yield return new object[]
        {
            new RegisterEmployeeDto()
            {
                Email = "test@test.pl",
                Password = "",
                FirstName = "",
                LastName = "Test"
            }
        };
        yield return new object[]
        {
            new RegisterEmployeeDto()
            {
            }
        };
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}