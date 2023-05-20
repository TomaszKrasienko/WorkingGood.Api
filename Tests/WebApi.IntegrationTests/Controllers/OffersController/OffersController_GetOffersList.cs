using System.Drawing;
using System.Web;
using Application.ViewModels.Offer;
using Domain.Models.Offer;
using FluentAssertions;
using Infrastructure.Common.ConfigModels;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace WebApi.IntegrationTests.Controllers.OffersController;
[Collection("WebApiTests")]
public class OffersController_GetOffersList : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public OffersController_GetOffersList(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions =
                        services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<WgDbContext>));
                    services.Remove(dbContextOptions!);
                    services.AddDbContext<WgDbContext>(options => options.UseInMemoryDatabase("WorkingGoodTests"));
                    // var jwtConfigDescriptor =
                    //     services.SingleOrDefault(x => x.ServiceType == typeof(JwtConfig));
                    //_jwtConfig = (jwtConfigDescriptor!.ImplementationInstance as JwtConfig)!;
                });  
            });
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetOffersList_ForPaginationPageNumber1AndPageSize10_ShouldReturnTenOffersFromFirstPageWithHeaderOfPagination()
    {
        //Arrange
        await SeedOffers();
        //Act
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["pageNumber"] = "1";
        query["PageSize"] = "10";
        string queryString = query.ToString();
        var response = await _client.GetAsync($"offers/getOffersList?{queryString}");
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var stringContent = await response.Content.ReadAsStringAsync();
        var objectResult = JsonConvert.DeserializeObject<List<GetOfferVM>>(stringContent);
        objectResult.Count().Should().Be(10);
        var headers = response.Headers
            .Where(x => x.Key == "X-Pagination")
            .Select(x => x.Value)
            .FirstOrDefault();
        headers.FirstOrDefault().Should().Be(
        "{\"totalCount\":20,\"pageSize\":10,\"currentPage\":1,\"totalPages\":2,\"hasNext\":true,\"hasPrevious\":false}");
    }
    private async Task SeedOffers()
    {
        List<Offer> offersList = new();
        for(int i = 0; i < 20; i++)
        {
            offersList.Add(
                new Offer(
                    "testTitle",
                    "testPositionType",
                    1000,
                    1000,
                    "descriptionTestdescriptionTestdescriptionTest",
                    Guid.NewGuid(),
                    true
                ));
        };
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory!.CreateScope();
        WgDbContext dbContext = scope.ServiceProvider.GetService<WgDbContext>()!;
        var existingOffers = await dbContext.Offers.ToListAsync();
        dbContext.Offers.RemoveRange(existingOffers);
        await dbContext.Offers.AddRangeAsync(offersList);
        await dbContext.SaveChangesAsync();
    }
    [Fact]
    public async Task GetOffersList_ForPaginationPageNumber1AndPageSize5AndIsActiveTrue_ShouldReturnTenOffersFromFirstPageWithHeaderOfPagination()
    {
        //Arrange
        await SeedOffersForPaginationPageNumber1AndPageSize5AndIsActiveTrue();
        //Act
        var query = HttpUtility.ParseQueryString(string.Empty);
        query["pageNumber"] = "1";
        query["PageSize"] = "5";
        query["IsActive"] = "true";
        string queryString = query.ToString();
        var response = await _client.GetAsync($"offers/getOffersList?{queryString}");
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var stringContent = await response.Content.ReadAsStringAsync();
        var objectResult = JsonConvert.DeserializeObject<List<GetOfferVM>>(stringContent);
        objectResult.Count().Should().Be(5);
        var headers = response.Headers
            .Where(x => x.Key == "X-Pagination")
            .Select(x => x.Value)
            .FirstOrDefault();
        headers.FirstOrDefault().Should().Be(
            "{\"totalCount\":5,\"pageSize\":5,\"currentPage\":1,\"totalPages\":1,\"hasNext\":false,\"hasPrevious\":false}");
    }
    private async Task SeedOffersForPaginationPageNumber1AndPageSize5AndIsActiveTrue()
    {
        List<Offer> offersList = new();
        for(int i = 0; i < 10; i++)
        {
            bool isActive = i % 2 == 0;
            offersList.Add(
                new Offer(
                    "testTitle",
                    "testPositionType",
                    1000,
                    1000,
                    "descriptionTestdescriptionTestdescriptionTest",
                    Guid.NewGuid(),
                    isActive
                ));
            
        };
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory!.CreateScope();
        WgDbContext dbContext = scope.ServiceProvider.GetService<WgDbContext>()!;
        var existingOffers = await dbContext.Offers.ToListAsync();
        dbContext.Offers.RemoveRange(existingOffers);
        await dbContext.Offers.AddRangeAsync(offersList);
        await dbContext.SaveChangesAsync();
    }
}

