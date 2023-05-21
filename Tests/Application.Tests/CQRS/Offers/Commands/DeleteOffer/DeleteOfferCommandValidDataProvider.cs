using System.Collections;
using Application.CQRS.Offers.Commands.DeleteOffer;

namespace Application.Tests.CQRS.Offers.Commands.DeleteOffer;

public class DeleteOfferCommandValidDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new[]
        {
            new DeleteOfferCommand
            {
                OfferId = Guid.NewGuid()
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}