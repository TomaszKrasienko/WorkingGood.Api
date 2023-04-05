using System.Collections;
using Application.CQRS.EmployeesAuth.Commands.ResetPassword;

namespace Application.Tests.CQRS.EmployeesAuth.Commands.ResetPassword;

public class ResetPasswordTestsDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new ResetPasswordCommand()
            {
                ResetPasswordDto = new()
                {
                    NewPassword = "test",
                    ConfirmNewPassword = "test",
                    ResetToken = "test"
                }
            }
        };
        yield return new object[]
        {
            new ResetPasswordCommand()
            {
                ResetPasswordDto = new()
                {
                    NewPassword = "TEST123",
                    ConfirmNewPassword = "TEST123",
                    ResetToken = "test"
                }
            }
        };
        yield return new object[]
        {
            new ResetPasswordCommand()
            {
                ResetPasswordDto = new()
                {
                    NewPassword = "Test123!",
                    ConfirmNewPassword = "Test321!",
                    ResetToken = "test"
                }
            }
        };
        yield return new object[]
        {
            new ResetPasswordCommand()
            {
                ResetPasswordDto = new()
                {
                    NewPassword = "Test123!",
                    ConfirmNewPassword = "Test123!",
                    ResetToken = ""
                }
            }
        };
        yield return new object[]
        {
            new ResetPasswordCommand()
            {
                ResetPasswordDto = new()
                {
                    NewPassword = "",
                    ConfirmNewPassword = "test",
                    ResetToken = "test"
                }
            }
        };
        yield return new object[]
        {
            new ResetPasswordCommand()
            {
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}