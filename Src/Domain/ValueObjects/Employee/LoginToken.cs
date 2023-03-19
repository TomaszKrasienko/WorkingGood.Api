namespace Domain.ValueObjects;

public class LoginToken
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}