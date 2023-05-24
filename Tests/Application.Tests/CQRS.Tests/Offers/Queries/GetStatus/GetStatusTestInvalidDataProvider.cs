using System.Collections;
using Application.CQRS.Offers.Queries.GetOfferStatus;

namespace Application.Tests.CQRS.Offers.Queries.GetStatus;

public class GetStatusTestInvalidDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new[]
        {
            new GetStatusQuery()
            {
            }
        };
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}