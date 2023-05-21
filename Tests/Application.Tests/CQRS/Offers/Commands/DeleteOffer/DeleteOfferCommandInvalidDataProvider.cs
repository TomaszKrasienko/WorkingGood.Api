using System.Collections;
using Application.CQRS.Offers.Commands.DeleteOffer;

namespace Application.Tests.CQRS.Offers.Commands.DeleteOffer;

public class DeleteOfferCommandInvalidDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new DeleteOfferCommand
            {

            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}