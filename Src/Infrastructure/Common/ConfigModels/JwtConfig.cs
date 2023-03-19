namespace Infrastructure.Common.ConfigModels;

public class JwtConfig
{
    public string TokenKey { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
}