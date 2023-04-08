using Domain.Models.Offer;

namespace Domain.Interfaces.Repositories;

public interface IOfferRepository : IRepository<Offer>
{
    Task<List<Offer>> GetAllForCompany(Guid AuthorId);
}