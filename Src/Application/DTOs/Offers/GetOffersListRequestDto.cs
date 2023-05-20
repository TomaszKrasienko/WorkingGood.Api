namespace Application.DTOs.Offers;

public record GetOffersListRequestDto : BaseParameters
{
    public bool? CompanyOffers { get; init; } = false;
    public bool? IsActive { get; init; }
    public bool? AuthorOffers { get; init; }
    public int? RateFrom{ get; init; }
    public int? RateTo { get; set; }
    public string? SearchPhrase { get; init; }
}