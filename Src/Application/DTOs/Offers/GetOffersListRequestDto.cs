namespace Application.DTOs.Offers;

public record GetOffersListRequestDto : BaseParameters
{
    public bool? CompanyOffers { get; init; } = false;
    public bool? IsActive { get; init; } 
}