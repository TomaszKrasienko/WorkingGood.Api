using System.Linq.Expressions;
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

    public async Task<List<Offer>> GetAllActive()
    {
        return await _context
            .Offers
            .Where(x => x.OfferStatus.IsActive == true)
            .ToListAsync();
    }
    public Task<List<Offer>> GetAllAsync(int pageNumber, int pageSize, List<Guid> employeeIdList, bool? isActive)
    {
        IQueryable<Offer> query = _context.Set<Offer>();
        if (employeeIdList!.Any())
        {
            query = query.Where(x => employeeIdList!.Contains(x.AuthorId));
        }
        if (isActive is not null)
        {
            query = query.Where(x => x.OfferStatus.IsActive == isActive);
        }
        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> CountAll(List<Guid> employeeIdList, bool? isActive)
    {
        IQueryable<Offer> query = _context.Set<Offer>();
        if (employeeIdList!.Any())
        {
            query = query.Where(x => employeeIdList!.Contains(x.AuthorId));
        }
        if (isActive is not null)
        {
            query = query.Where(x => x.OfferStatus.IsActive == isActive);
        }
        return await query.CountAsync();
    }
}