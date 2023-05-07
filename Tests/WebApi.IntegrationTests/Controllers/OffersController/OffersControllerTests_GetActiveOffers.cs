using Application.ViewModels.Offer;
using Domain.Models.Offer;
using FluentAssertions;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.IntegrationTests.Tests.Helpers;

namespace WebApi.IntegrationTests.Controllers.OffersController;

public class OffersControllerTests_GetActiveOffers : IClassFixture<WebApplicationFactory<Program>>
{    
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    public OffersControllerTests_GetActiveOffers(WebApplicationFactory<Program> factory)
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
    public async Task GetActiveOffers_ForExistingOffers_ShouldReturnOk()
    {
        //Arrange
        await SeedOffers();
        //Act
        var response = await _client.GetAsync("offers/getActiveOffers");
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var stringContent = await response.Content.ReadAsStringAsync();
        //stringContent.GetBaseMessageDto().Object.Should().BeOfType<List<GetOfferVM>>();
        var objectResult = stringContent.GetBaseMessageDto().Object;
        (objectResult as List<GetOfferVM>).Count().Should().Be(2);
    }
    private async Task SeedOffers()
    {
        List<Offer> offersList = new()
        {
            new Offer(
                "testTitle",
                "testPositionType",
                1000,
                1000,
                "descriptionTestdescriptionTestdescriptionTest",
                Guid.NewGuid(),
                true
            ),
            new Offer(
                "testTitle",
                "testPositionType",
                1000,
                1000,
                "descriptionTestdescriptionTestdescriptionTest",
                Guid.NewGuid(),
                true
            ),
            new Offer(
                "testTitle",
                "testPositionType",
                1000,
                1000,
                "descriptionTestdescriptionTestdescriptionTest",
                Guid.NewGuid(),
                false
            )
        };
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory!.CreateScope();
        WgDbContext dbContext = scope.ServiceProvider.GetService<WgDbContext>()!;
        await dbContext.Offers.AddRangeAsync(offersList);
        await dbContext.SaveChangesAsync();
    }
}