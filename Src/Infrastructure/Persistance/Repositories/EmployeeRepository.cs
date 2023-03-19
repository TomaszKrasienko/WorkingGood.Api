using Domain.Interfaces.Repositories;
using Domain.Models.Employee;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repositories;

public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(WgDbContext context) : base(context)
    {
    }
    public async Task<Employee> GetByVerificationToken(string token)
    {
        return await _context
            .Employees
            .FirstOrDefaultAsync(x =>
                x.VerificationToken.Token == token);
    }
    public async Task<Employee> GetByEmail(string email)
    {        
        return await _context
            .Employees
            .FirstOrDefaultAsync(x =>
                x.Email == email);
    }

    public async Task<Employee> GetByRefreshToken(string refreshToken)
    {
        return await _context
            .Employees
            .FirstOrDefaultAsync(x =>
                x.RefreshToken.Token == refreshToken);
    }
}