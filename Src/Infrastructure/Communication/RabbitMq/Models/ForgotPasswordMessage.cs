using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Communication.Models;

public class ForgotPasswordMessage
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string ForgotPasswordToken { get; set; } = string.Empty;
}