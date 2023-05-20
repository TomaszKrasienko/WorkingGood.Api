using System.Collections;
using Application.CQRS.Offers.Commands;

namespace Application.Tests.CQRS.Offers.Commands.AddOffer;

public class AddOfferTestInvalidDataProvider : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new[]
        {
            new AddOfferCommand
            {
                OfferDto = new()
                {
                    Title = "Title",
                    Description = "Description description Description description",
                    PositionType = "Test",
                    SalaryRangeMax = 12000,
                    SalaryRangeMin = 10000
                }
            }
        };
        yield return new[]
        {
            new AddOfferCommand
            {
                EmployeeId = Guid.NewGuid()
            }
        };
        yield return new[]
        {
            new AddOfferCommand
            {
                EmployeeId = Guid.NewGuid(),
                OfferDto = new()
                {
                    Title = "",
                    Description = "Description description Description description",
                    PositionType = "Test",
                    SalaryRangeMax = 12000,
                    SalaryRangeMin = 10000
                }
            }
        };
        yield return new[]
        {
            new AddOfferCommand
            {
                EmployeeId = Guid.NewGuid(),
                OfferDto = new()
                {
                    Title = "Title",
                    Description = "Description description Description description",
                    PositionType = "",
                    SalaryRangeMax = 12000,
                    SalaryRangeMin = 10000
                }
            }
        };
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
                    SalaryRangeMin = 10000
                }
            }
        };
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
                    SalaryRangeMax = 12000
                }
            }
        };
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
                    SalaryRangeMax = 12000,
                    SalaryRangeMin = 300
                }
            }
        };
        yield return new[]
        {
            new AddOfferCommand
            {
                EmployeeId = Guid.NewGuid(),
                OfferDto = new()
                {
                    Title = "Title",
                    Description = "",
                    PositionType = "Test",
                    SalaryRangeMax = 12000,
                    SalaryRangeMin = 10000
                }
            }
        };
        yield return new[]
        {
            new AddOfferCommand
            {
                EmployeeId = Guid.NewGuid(),
                OfferDto = new()
                {
                    Title = "Title",
                    Description = "Description",
                    PositionType = "Test",
                    SalaryRangeMax = 12000,
                    SalaryRangeMin = 10000
                }
            }
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}