using Domain.Interfaces.Validation;
using FluentValidation;

namespace Application.CQRS.EmployeesAuth.Commands.ResetPassword;

public class ResetPasswordValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordValidator(IEmployeeChecker employeeChecker)
    {
        RuleFor(x => x.ResetPasswordDto)
            .NotNull();
        When(x => x.ResetPasswordDto != null, () =>
        {
            RuleFor(x => x.ResetPasswordDto.NewPassword)
                .NotNull()
                .NotEmpty();
            When(x => x.ResetPasswordDto.NewPassword != null, () =>
            {
                RuleFor(x => x.ResetPasswordDto.NewPassword)
                    .Must(x => IsPasswordValid(x))
                    .WithMessage("New password is invalid");
            });
            RuleFor(x => x.ResetPasswordDto.ConfirmNewPassword)
                .NotNull()
                .NotEmpty();
            When(x => x.ResetPasswordDto.ConfirmNewPassword != null, () =>
            {
                RuleFor(x => new {x.ResetPasswordDto.NewPassword, x.ResetPasswordDto.ConfirmNewPassword})
                    .Must(x => IsConfirmMatched(x.NewPassword, x.ConfirmNewPassword))
                    .WithMessage("Password not match to confirmation");
            });
            RuleFor(x => x.ResetPasswordDto.ResetToken)
                .NotNull()
                .NotEmpty();
            When(x => x.ResetPasswordDto.ResetToken != null, () =>
            {
                RuleFor(x => x.ResetPasswordDto.ResetToken)
                    .Must(x => employeeChecker.IsResetTokenExists(x) == true)
                    .WithMessage("Not found employee");
            });
        });
    }
    //Todo: wydzielić te metody do innych serwisów
    private bool IsPasswordValid(string password)
    {
        if (!(password.Any(char.IsUpper)) || !(password.Any(char.IsLower)) || !(password.Any(char.IsNumber)))
            return false;
        return true;
    }
    private bool IsConfirmMatched(string password, string confirmation)
    {
        return password.Equals(confirmation);
    }
}