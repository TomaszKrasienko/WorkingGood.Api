using System.Collections;
using Application.CQRS.Offers.Queries.GetAllForCompany;

namespace Application.Tests.CQRS.Offers.Queries.GetAllForCompany;

public class GetAllForCompanyTestInvalidDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new GetAllForCompanyQuery()
            {
            }
        };
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}