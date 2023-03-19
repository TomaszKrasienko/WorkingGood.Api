using Domain.Interfaces.Validation;
using Infrastructure.Persistance;

namespace Infrastructure.Validation;

public class CompanyChecker : ICompanyChecker
{
    private readonly WgDbContext _context;
    public CompanyChecker(WgDbContext context)
    {
        _context = context;
    }
    public bool IsCompanyExists(Guid companyId)
    {
        return _context.Companies.Any(x => x.Id == companyId);
    }
}