using System.Collections;
using Application.CQRS.Offers.Queries.GetOffersList;

namespace Application.Tests.CQRS.Offers.Queries.GetOffersList;

public class GetOffersListQueryValidDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new GetOffersListQuery()
            {
                EmployeeId = Guid.NewGuid(),
                GetOffersListRequestDto = new()
                {
                    CompanyOffers = true,
                    IsActive = true,
                    PageNumber = 1,
                    PageSize = 15
                }
            }
        };
        yield return new object[]
        {
            new GetOffersListQuery()
            {
                GetOffersListRequestDto = new()
                {
                    CompanyOffers = false,
                    IsActive = true,
                    PageNumber = 1,
                    PageSize = 15
                }
            }
        };
        yield return new object[]
        {
            new GetOffersListQuery()
            {
                GetOffersListRequestDto = new()
                {
                    CompanyOffers = false,
                    PageNumber = 1,
                    PageSize = 15
                }
            }
        };
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}