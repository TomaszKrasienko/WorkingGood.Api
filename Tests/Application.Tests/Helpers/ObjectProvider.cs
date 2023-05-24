using Application.ViewModels.Offer;
using Domain.Models.Offer;

namespace Application.Tests.Helpers;

internal static class ObjectProvider
{
    internal static Offer GetOffer()
    {
        Offer offer = new(
            "Title test",
            "Test position type",
            10000,
            15000,
            "Description test Description test Description test",
            Guid.NewGuid(),
            false);
        return offer;
    }

    internal static GetOfferVM GetGetOfferVm()
    {
        return new GetOfferVM()
        {
            Id = Guid.NewGuid(),
            Title = "Title test",
            PositionType = "Test position type",
            SalaryRangeMin = 10000,
            SalaryRangeMax = 15000,
            Description = "Description test Description test Description test",
            IsActive = false
        };
    }
}