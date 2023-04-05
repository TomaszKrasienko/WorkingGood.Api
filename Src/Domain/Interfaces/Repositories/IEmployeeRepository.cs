using Domain.Models.Employee;

namespace Domain.Interfaces.Repositories;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Employee> GetByVerificationTokenAsync(string token);
    Task<Employee> GetByEmailAsync(string email);
    Task<Employee> GetByRefreshTokenAsync(string refreshToken);
    Task<Employee> GetByResetToken(string token);
}