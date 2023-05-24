using System.Collections;
using Application.CQRS.Offers.Queries.GetOffersList;

namespace Application.Tests.CQRS.Offers.Queries.GetOffersList;

public class GetOffersListQueryInValidDataProvider: IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new GetOffersListQuery()
            {
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
                EmployeeId = default(Guid),
                GetOffersListRequestDto = new()
                {
                    CompanyOffers = true,
                    IsActive = true,
                    PageNumber = 1,
                    PageSize = 15
                }
            }
        };
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}