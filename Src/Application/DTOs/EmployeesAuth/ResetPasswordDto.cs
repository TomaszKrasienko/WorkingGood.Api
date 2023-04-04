namespace Application.DTOs.EmployeesAuth;

public class ResetPasswordDto
{
    public string? ResetToken { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmNewPassword { get; set; }
}