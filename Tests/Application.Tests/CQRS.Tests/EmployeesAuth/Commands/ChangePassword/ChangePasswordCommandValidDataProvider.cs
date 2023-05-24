using System.Collections;
using Application.CQRS.EmployeesAuth.Commands.ChangePassword;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.ChangePassword;

public class ChangePasswordCommandValidDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new[]
        {
            new ChangePasswordCommand()
            {
                EmployeeId = Guid.NewGuid(),
                ChangePasswordDto = new()
                {
                    NewPassword = "TestPass123",
                    OldPassword = "TestOldPass123!",
                    ConfirmNewPassword = "TestPass123"
                }
            }
        };
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}