using Infrastructure.Persistance;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.IntegrationTests.Controllers.EmployeesAuthController;

public class EmployeeAuthControllerTests_Refresh : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    public EmployeeAuthControllerTests_Refresh(WebApplicationFactory<Program> factory)
    {
        // JwtConfig jwtConfig = new()
        // {
        //     TokenKey = "my top secret key",
        //     Audience = "test",
        //     Issuer = "test"
        // };
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<WgDbContext>));
                    services.Remove(dbContextOptions!);
                    services.AddDbContext<WgDbContext>(options => options.UseInMemoryDatabase("WorkingGoodTests"));
                    // services.AddSingleton(jwtConfig);
                });
            });
        _client = _factory.CreateClient();
    }
}