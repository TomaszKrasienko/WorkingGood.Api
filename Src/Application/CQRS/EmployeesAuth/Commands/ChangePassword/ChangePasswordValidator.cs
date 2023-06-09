using Application.DTOs.EmployeesAuth;
using Domain.Interfaces.Validation;
using FluentValidation;

namespace Application.CQRS.EmployeesAuth.Commands.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator(IEmployeeChecker employeeChecker)
    {
        RuleFor(x => x.ChangePasswordDto)
            .NotNull();
        RuleFor(x => x.ChangePasswordDto.NewPassword)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.ChangePasswordDto.NewPassword)
            .Must(x => IsPasswordValid(x) == true)
            .WithMessage("Password must contains uppercase, lowercase and number")
            .When(x => x.ChangePasswordDto.NewPassword != null);
        RuleFor(x => x.ChangePasswordDto.OldPassword)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.ChangePasswordDto.ConfirmNewPassword)
            .NotNull()
            .NotEmpty();
        When(x => IsConfirmNull(x.ChangePasswordDto) == false, () => {
            RuleFor(x => new {x.ChangePasswordDto.NewPassword, x.ChangePasswordDto.ConfirmNewPassword})
                .Must(x => IsConfirmMatched(x.NewPassword!, x.ConfirmNewPassword!))
                .WithMessage("Password not match to confirmation");
        });
        RuleFor(x => x.EmployeeId)
            .NotNull();
        When(x => x.EmployeeId != null, () =>
        {
            RuleFor(x => x.EmployeeId)
                .NotEqual(default(Guid));
            RuleFor(x => x.EmployeeId)
                .Must(x => employeeChecker.IsEmployeeExists((Guid) x!))
                .WithMessage("Employee is not exists");
        });
    }
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

    private bool IsConfirmNull(ChangePasswordDto changePasswordDto)
    {
        return changePasswordDto.NewPassword == null || changePasswordDto.ConfirmNewPassword == null;
    }
}