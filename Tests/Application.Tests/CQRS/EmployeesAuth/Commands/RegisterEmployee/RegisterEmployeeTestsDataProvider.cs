using System.Collections;
using Application.EmployeesAuth.Commands;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.RegisterEmployee;

public class RegisterEmployeeTestsDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new RegisterEmployeeCommand()
            {
                RegisterEmployeeDto = new()
                {
                    Email = "test@test.pl",
                    Password = "Test123",
                    FirstName = "Test",
                    LastName = "Test"
                }
            }
        };        
        yield return new object[]
        {
            new RegisterEmployeeCommand()
            {
                CompanyId = Guid.NewGuid(),
                RegisterEmployeeDto = new()
                {
                    Email = "testpl",
                    Password = "Test123",
                    FirstName = "Test",
                    LastName = "Test"
                }
            }
        };
        yield return new object[]
        {
            new RegisterEmployeeCommand()
            {
                CompanyId = Guid.NewGuid(),
                RegisterEmployeeDto = new()
                {
                    Email = "test@test.pl",
                    Password = "test123",
                    FirstName = "Test",
                    LastName = "Test"
                }
            }
        };
        yield return new object[]
        {
            new RegisterEmployeeCommand()
            {
                CompanyId = Guid.NewGuid(),
                RegisterEmployeeDto = new()
                {
                    Email = "test@test.pl",
                    Password = "",
                    FirstName = "",
                    LastName = "Test"
                }
            }
        };
        yield return new object[]
        {
            new RegisterEmployeeCommand()
            {
                CompanyId = Guid.NewGuid()
            }
        };
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}