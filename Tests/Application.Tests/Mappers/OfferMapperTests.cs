using Application.Common.Mapper;
using Application.ViewModels.Offer;
using AutoMapper;
using Domain.Models.Offer;
using FluentAssertions;

namespace Application.Tests.Mappers;

public class OfferMapperTests
{
    [Fact]
    public void GetOfferVmMap_ForNonActiveOffer_ShouldReturnGetOfferVm()
    {
        //Arrange
        Offer offer = new Offer("title test", "test position", 10000, 20000, "description", Guid.NewGuid(), false);
        var config = new MapperConfiguration(cfg => cfg.AddProfile<OfferMapperProfile>());
        var mapper = config.CreateMapper();
        //Act
        GetOfferVM getEmployeeVm = mapper.Map<GetOfferVM>(offer);
        //Assert
        getEmployeeVm.Id.Should().Be(offer.Id);
        getEmployeeVm.Title.Should().Be(offer.Content.Title);
        getEmployeeVm.PositionType.Should().Be(offer.Position.Type);
        getEmployeeVm.SalaryRangeMin.Should().Be(offer.SalaryRanges.ValueMin);
        getEmployeeVm.SalaryRangeMax.Should().Be(offer.SalaryRanges.ValueMax);
        getEmployeeVm.IsActive.Should().Be(offer.OfferStatus.IsActive);
        getEmployeeVm.Description.Should().Be(offer.Content.Description);
    }
    [Fact]
    public void GetOfferVmMap_ForActiveOffer_ShouldReturnGetOfferVm()
    {
        //Arrange
        Offer offer = new Offer("title test", "test position", 10000, 20000, "description", Guid.NewGuid(), true);
        var config = new MapperConfiguration(cfg => cfg.AddProfile<OfferMapperProfile>());
        var mapper = config.CreateMapper();
        //Act
        GetOfferVM getEmployeeVm = mapper.Map<GetOfferVM>(offer);
        //Assert
        getEmployeeVm.Id.Should().Be(offer.Id);
        getEmployeeVm.Title.Should().Be(offer.Content.Title);
        getEmployeeVm.PositionType.Should().Be(offer.Position.Type);
        getEmployeeVm.SalaryRangeMin.Should().Be(offer.SalaryRanges.ValueMin);
        getEmployeeVm.SalaryRangeMax.Should().Be(offer.SalaryRanges.ValueMax);
        getEmployeeVm.IsActive.Should().Be(offer.OfferStatus.IsActive);
        getEmployeeVm.Description.Should().Be(offer.Content.Description);
    }
}