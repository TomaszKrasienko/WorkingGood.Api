using System.Collections;
using Application.CQRS.Offers.Commands.ChangeOfferStatus;

namespace Application.Tests.CQRS.Offers.Commands.ChangeOfferStatus;

public class ChangeOfferStatusCommandValidDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new[]
        {
            new ChangeOfferStatusCommand
            {
                OfferId = Guid.NewGuid()
            }
        };
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}