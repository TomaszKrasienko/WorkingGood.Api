using System.Collections;
using Application.CQRS.EmployeesAuth.Commands.ForgotPassword;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.ForgotPassword;

public class ForgotPasswordCommandValidDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new[]
        {
            new ForgotPasswordCommand()
            {
                ForgotPasswordDto = new()
                {
                    EmployeeEmail = "test@test.pl"
                }
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator() =>  GetEnumerator();
}