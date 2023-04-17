using Application.DTOs;
using Application.DTOs.Offers;
using Domain.Models.Employee;
using Domain.Models.Offer;
using FluentAssertions;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WebApi.IntegrationTests.Tests.Helpers;

namespace WebApi.IntegrationTests.Controllers.OffersController;

public class OffersControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    public OffersControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions =
                        services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<WgDbContext>));
                    services.Remove(dbContextOptions!);
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddMvc(options => options.Filters.Add(new FakeUserFilter()));
                    services.AddDbContext<WgDbContext>(options => options.UseInMemoryDatabase("WorkingGoodTests"));
                });
            });
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task AddOffer_ForValidOfferDto_ShouldReturnOkResult()
    {
        //Arrange
        OfferDto offerDto = new()
        {
            Title = "Test title",
            Description = "Description test Description test Description test Description test",
            PositionType = "Position type test",
            SalaryRangeMax = 12000,
            SalaryRangeMin = 8000
        };
        var httpContent = offerDto.ToJsonContent();
        //Act
        var response = await _client.PostAsync("api/Offers/AddOffer", httpContent);
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task? GetOfferStatus_ForExistingOfferShouldReturnOk(bool isActive)
    {
        //Arrange
        Guid offerId = await SeedOffer(isActive);
        //Act
        var response = await _client.GetAsync($"/api/Offers/GetOfferStatus/{offerId}");
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var stringContent = await response.Content.ReadAsStringAsync();
        stringContent.GetBaseMessageDto().Object.Should().Be(isActive);
    }
    private async Task<Guid> SeedOffer(bool isActive)
    {
        Offer offer = new Offer(
            "testTitle",
            "testPositionType",
            1000,
            1000,
            "descriptionTestdescriptionTestdescriptionTest",
            Guid.NewGuid(),
            isActive
        );
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory!.CreateScope();
        WgDbContext dbContext = scope.ServiceProvider.GetService<WgDbContext>()!;
        await dbContext.Offers.AddAsync(offer);
        await dbContext.SaveChangesAsync();
        return offer.Id;
    }


}