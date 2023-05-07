using Domain.Interfaces.Validation;
using Infrastructure.Persistance;

namespace Infrastructure.Validation;

public class OfferChecker : IOfferChecker
{
    private readonly WgDbContext _context;
    public OfferChecker(WgDbContext context)
    {
        _context = context;
    }
    public bool IsOfferExists(Guid offerId)
    {
        return _context
            .Offers
            .Any(x => x.Id == offerId);
    }

    public bool IsOfferActive(Guid offerId)
    {
        return _context
            .Offers
            .Any(x => x.Id == offerId && x.OfferStatus.IsActive == true);
    }
}