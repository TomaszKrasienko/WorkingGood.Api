using Domain.Interfaces.Repositories;
using Domain.Models.Employee;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories;

public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(WgDbContext context) : base(context)
    {
    }
    public async Task<Employee> GetByVerificationTokenAsync(string token)
    {
        return await _context
            .Employees
            .FirstOrDefaultAsync(x =>
                x.VerificationToken.Token == token);
    }
    public async Task<Employee> GetByEmailAsync(string email)
    {        
        return await _context
            .Employees
            .FirstOrDefaultAsync(x =>
                x.Email == email);
    }

    public async Task<Employee> GetByRefreshTokenAsync(string refreshToken)
    {
        return await _context
            .Employees
            .FirstOrDefaultAsync(x =>
                x.RefreshToken.Token == refreshToken);
    }

    public async Task<Employee> GetByResetToken(string token)
    {
        return await _context
            .Employees
            .FirstOrDefaultAsync(x => x.ResetToken.Token == token);
    }

    public async Task<List<Employee>> GetByCompanyIdAsync(Guid companyId)
    {
        return await _context
            .Employees
            .Where(x => x.CompanyId == companyId)
            .ToListAsync();
    }
}