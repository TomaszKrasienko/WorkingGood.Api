using Domain.Interfaces.Repositories;
using Domain.Models.Offer;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories;

public class OfferRepository : BaseRepository<Offer>, IOfferRepository
{
    public OfferRepository(WgDbContext context) : base(context)
    {
    }
    public async Task<List<Offer>> GetAllForEmployees(List<Guid> employeesId)
    {
        return await _context
            .Offers
            .Where(x => employeesId.Contains(x.AuthorId))
            .ToListAsync();
    }
}