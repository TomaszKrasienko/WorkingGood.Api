using Domain.Interfaces.Validation;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

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

    public bool IsCompanyExists(string name)
    {
        return _context.Companies.Any(x => x.Name == name);
    }
}