using System.Linq.Expressions;
using Domain.Models.Offer;

namespace Domain.Interfaces.Repositories;

public interface IOfferRepository : IRepository<Offer>
{
    Task<List<Offer>> GetAllForEmployees(List<Guid> employeesId);
    Task<List<Offer>> GetAllActive();
    Task<List<Offer>> GetAllAsync(
        int pageNumber, 
        int pageSize, 
        List<Guid>? employeeIdList, 
        bool? isActive, 
        Guid? employeeId,
        int? rateFrom,
        int? rateTo,
        string? searchPhrase);
    Task<int> CountAll(List<Guid> employeeIdList, bool? isActive);
}