
using Domain.Interfaces.Repositories;
using Domain.Models.Company;

namespace Infrastructure.Persistance.Repositories
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(WgDbContext context) : base(context)
        {
        }
    }
}

