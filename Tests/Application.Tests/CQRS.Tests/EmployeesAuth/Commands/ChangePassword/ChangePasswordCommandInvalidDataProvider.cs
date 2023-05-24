using System.Collections;
using Application.CQRS.EmployeesAuth.Commands.ChangePassword;
using Application.Tests.Helpers;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.ChangePassword;

public class ChangePasswordCommandInvalidDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new[]
        {
            new ChangePasswordCommand()
            {
                ChangePasswordDto = new()
                {
                    NewPassword = "NewPass123",
                    OldPassword = "OldPass123",
                    ConfirmNewPassword = "NewPass123"
                }
            }
        };
        yield return new[]
        {
            new ChangePasswordCommand()
            {
                EmployeeId = default(Guid),
                ChangePasswordDto = new()
                {
                    NewPassword = "NewPass123",
                    OldPassword = "OldPass123",
                    ConfirmNewPassword = "NewPass123"
                }
            }
        };
        yield return new[]
        {
            new ChangePasswordCommand()
            {
                EmployeeId = Guid.NewGuid(),
                ChangePasswordDto = new()
                {
                    NewPassword = "NewPass123",
                    OldPassword = "OldPass123",
                    ConfirmNewPassword = "WrongPass123"
                }
            }
        };
        yield return new[]
        {
            new ChangePasswordCommand()
            {
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}