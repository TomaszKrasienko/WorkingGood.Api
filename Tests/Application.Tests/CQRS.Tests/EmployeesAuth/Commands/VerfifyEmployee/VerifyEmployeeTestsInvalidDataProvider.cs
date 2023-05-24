using System.Collections;
using Application.CQRS.EmployeesAuth.Commands.VerifyEmployee;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.VerfifyEmployee;

public class VerifyEmployeeTestsInvalidDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new VerifyEmployeeCommand()
            {
                VerificationToken = ""
            }
        };
        yield return new object[]
        {
            new VerifyEmployeeCommand()
        };
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}