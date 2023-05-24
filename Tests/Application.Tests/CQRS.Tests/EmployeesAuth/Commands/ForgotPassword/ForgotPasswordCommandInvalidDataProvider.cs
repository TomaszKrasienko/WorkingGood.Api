using System.Collections;
using Application.CQRS.EmployeesAuth.Commands.ForgotPassword;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.ForgotPassword;

public class ForgotPasswordCommandInvalidDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new[]
        {
            new ForgotPasswordCommand()
            {
                ForgotPasswordDto = new()
            }
        };
        yield return new[]
        {
            new ForgotPasswordCommand()
            {
                
            }
        };
        yield return new[]
        {
            new ForgotPasswordCommand()
            {
                ForgotPasswordDto = new()
                {
                    EmployeeEmail = "notemail"
                }
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}