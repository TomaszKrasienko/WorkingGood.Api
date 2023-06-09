using System.Collections;
using Application.CQRS.Offers.Commands;
using Application.CQRS.Offers.Commands.EditOffer;

namespace Application.Tests.CQRS.Offers.Commands.EditOffer;

public class EditOfferTestInvalidDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new[]
        {
            new EditOfferCommand
            {
                OfferDto = new()
                {
                    Title = "Title",
                    Description = "Description description Description description",
                    SalaryRangeMax = 12000,
                    SalaryRangeMin = 10000
                }
            }
        };
        yield return new[]
        {
            new EditOfferCommand
            {
                OfferId = Guid.NewGuid()
            }
        };
        yield return new[]
        {
            new EditOfferCommand
            {
                OfferId = Guid.NewGuid(),
                OfferDto = new()
                {
                    Title = "",
                    Description = "Description description Description description",
                    SalaryRangeMax = 12000,
                    SalaryRangeMin = 10000
                }
            }
        };
        yield return new[]
        {
            new EditOfferCommand
            {
                OfferId = Guid.NewGuid(),
                OfferDto = new()
                {
                    Title = "Title",
                    Description = "Description description Description description",
                    SalaryRangeMax = 12000,
                    SalaryRangeMin = 10000
                }
            }
        };
        yield return new[]
        {
            new EditOfferCommand
            {
                OfferId = Guid.NewGuid(),
                OfferDto = new()
                {
                    Title = "Title",
                    Description = "Description description Description description",
                    SalaryRangeMin = 10000
                }
            }
        };
        yield return new[]
        {
            new EditOfferCommand
            {
                OfferId = Guid.NewGuid(),
                OfferDto = new()
                {
                    Title = "Title",
                    Description = "Description description Description description",
                    SalaryRangeMax = 12000
                }
            }
        };
        yield return new[]
        {
            new EditOfferCommand
            {
                OfferId = Guid.NewGuid(),
                OfferDto = new()
                {
                    Title = "Title",
                    Description = "Description description Description description",
                    SalaryRangeMax = 12000,
                    SalaryRangeMin = 300
                }
            }
        };
        yield return new[]
        {
            new EditOfferCommand
            {
                OfferId = Guid.NewGuid(),
                OfferDto = new()
                {
                    Title = "Title",
                    Description = "",
                    SalaryRangeMax = 12000,
                    SalaryRangeMin = 10000
                }
            }
        };
        yield return new[]
        {
            new EditOfferCommand
            {
                OfferId = Guid.NewGuid(),
                OfferDto = new()
                {
                    Title = "Title",
                    Description = "Description",
                    SalaryRangeMax = 12000,
                    SalaryRangeMin = 10000
                }
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}