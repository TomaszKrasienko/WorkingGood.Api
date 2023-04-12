using System.Net.Http.Headers;
using Domain.Models.Employee;
using Domain.ValueObjects;
using FluentAssertions;
using Infrastructure.Common.ConfigModels;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.IntegrationTests.Tests.Helpers;

namespace WebApi.IntegrationTests.Controllers.OffersController;

public class OffersControllerTests_GetAllForCompany : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private JwtConfig _jwtConfig;
    private readonly HttpClient _client;
    public OffersControllerTests_GetAllForCompany(WebApplicationFactory<Program> factory)
    {

        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions =
                        services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<WgDbContext>));
                    services.Remove(dbContextOptions!);
                    //services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    //services.AddMvc(options => options.Filters.Add(new FakeUserFilter()));
                    services.AddDbContext<WgDbContext>(options => options.UseInMemoryDatabase("WorkingGoodTests"));
                    var jwtConfigDescriptor =
                        services.SingleOrDefault(x => x.ServiceType == typeof(JwtConfig));
                    _jwtConfig = jwtConfigDescriptor.ImplementationInstance as JwtConfig;
                });
            });
        _client = _factory.CreateClient();
    }
    [Fact]
    public async Task GetAllForCompany_ForLoginEmployee_ShouldReturnOkResult()
    {
        //Arrange
        string token = await SeedEmployee();
        //Act
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        var response = await _client.GetAsync("api/Offers/GetAllForCompany");
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
    private async Task<string> SeedEmployee()
    {
        Employee employee = new Employee("Test", "Test", "test@test.pl", "Test123!", Guid.NewGuid());
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory!.CreateScope();
        WgDbContext dbContext = scope.ServiceProvider.GetService<WgDbContext>()!;
        await dbContext.Employees.AddAsync(employee);
        await dbContext.SaveChangesAsync();
        employee.Activate();
        await dbContext.SaveChangesAsync();
        LoginToken token = employee.Login("Test123!", _jwtConfig.TokenKey, _jwtConfig.Audience, _jwtConfig.Issuer);
        return token.Token;
    }
}