namespace Application.DTOs.Offers;

public class OfferDto
{
    public string? Title { get; init; }
    public string? PositionType { get; init; }
    public double? SalaryRangeMin { get; init; }
    public double? SalaryRangeMax { get; init; }
    public string? Description { get; init; }
    public bool? IsActive { get; init; }
}