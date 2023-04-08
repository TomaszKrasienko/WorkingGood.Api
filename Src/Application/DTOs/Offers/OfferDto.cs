namespace Application.DTOs.Offers;

public class OfferDto
{
    public string? Title { get; set; }
    public string? PositionType { get; set; }
    public double? SalaryRangeMin { get; set; }
    public double? SalaryRangeMax { get; set; }
    public string? Description { get; set; }
}