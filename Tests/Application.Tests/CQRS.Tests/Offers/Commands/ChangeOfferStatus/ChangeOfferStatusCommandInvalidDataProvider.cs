using System.Collections;
using Application.CQRS.Offers.Commands.ChangeOfferStatus;

namespace Application.Tests.CQRS.Offers.Commands.ChangeOfferStatus;

public class ChangeOfferStatusCommandInvalidDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new[]
        {
            new ChangeOfferStatusCommand
            {
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}