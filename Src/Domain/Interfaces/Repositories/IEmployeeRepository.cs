using Domain.Models.Employee;

namespace Domain.Interfaces.Repositories;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<Employee> GetByVerificationToken(string token);
    Task<Employee> GetByEmail(string email);
    Task<Employee> GetByRefreshToken(string refreshToken);
}