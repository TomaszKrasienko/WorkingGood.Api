namespace Application.DTOs.EmployeesAuth;

public class ChangePasswordDto
{
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmNewPassword { get; set; }
}