using System.Collections;
using Application.CQRS.Offers.Commands;

namespace Application.Tests.CQRS.Offers.Commands.AddOffer;

public class AddOfferCommandValidDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new[]
        {
            new AddOfferCommand
            {
                EmployeeId = Guid.NewGuid(),
                OfferDto = new()
                {
                    Title = "Title",
                    Description = "Description description Description description",
                    PositionType = "Test",
                    SalaryRangeMin = 10000,
                    SalaryRangeMax = 12000,
                    IsActive = true
                }
            }
        };
    }
    IEnumerator IEnumerable.GetEnumerator() =>  GetEnumerator();
    
}