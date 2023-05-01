namespace Infrastructure.Communication.Models;

public class RegisterMessage
{
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string RegistrationUrl { get; set; } = string.Empty;
}