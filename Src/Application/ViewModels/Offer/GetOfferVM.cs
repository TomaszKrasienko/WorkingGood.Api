namespace Application.ViewModels.Offer;

public class GetOfferVM
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string PositionType { get; set; } = string.Empty;
    public double SalaryRangeMin { get; set; }
    public double SalaryRangeMax { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}