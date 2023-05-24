using System.Collections;
using Application.CQRS.Offers.Commands.EditOffer;
using Application.DTOs.Offers;

namespace Application.Tests.CQRS.Offers.Commands.EditOffer;

public class EditOfferTestValidDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new[]
        {

            new EditOfferCommand()
            {
                OfferId = Guid.NewGuid(),
                OfferDto = new()
                {
                    Title = "newTestTitle",
                    Description = "newTestDescriptionnewTestDescriptionnewTestDescriptionnewTestDescription",
                    SalaryRangeMin = 10000,
                    SalaryRangeMax = 12000,
                    IsActive = true
                }
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
}