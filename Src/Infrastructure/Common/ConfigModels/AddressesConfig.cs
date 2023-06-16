namespace Infrastructure.Common.ConfigModels;

public record AddressesConfig
{
    public string VerifyUrl { get; init; } = string.Empty;
    public string ForgotPasswordUrl { get; init; } = string.Empty;
}