namespace Domain.Interfaces.Validation;

public interface IOfferChecker
{
    bool IsOfferExists(Guid offerId);
    bool IsOfferActive(Guid offerId);
}