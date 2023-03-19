namespace Application.ViewModels.Login;

public class LoginVM
{
    public string Token { get; set; } = String.Empty;
    public DateTime TokenExpiration { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiration { get; set; }
}