namespace Application.DTOs.Offers;

public record EditOfferRequestDto
{
    public string Title { get; init; } = string.Empty;
    public double SalaryRangeMin { get; init; }
    public double SalaryRangeMax { get; init; }
    public string Description { get; init; } = string.Empty;
    public bool IsActive { get; init; }
}