namespace Application.ViewModels.Company;

public record CompanyVM
{
    public string Name { get; init; } = string.Empty;
    public string Logo { get; init; } = string.Empty;
}