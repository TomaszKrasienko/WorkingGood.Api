using Domain.Interfaces.Repositories;
using Domain.Models.Offer;

namespace Infrastructure.Persistance.Repositories;

public class OfferRepository : BaseRepository<Offer>, IOfferRepository
{
    public OfferRepository(WgDbContext context) : base(context)
    {
    }

    public async Task<List<Offer>> GetAllForCompany(Guid AuthorId)
    {
        // var authorsList = await _context.Employees.Where()
        // return await _context
        //     .Offers 
        //     .Where(x => )
    }
}