using Domain.Models.Employee;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.IntegrationTests.Controllers.EmployeeController;

[Collection("WebApiTests")]
public class EmployeeControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public EmployeeControllerTests(WebApplicationFactory<Program> factory)
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
                });
            });
        _client = _factory.CreateClient();
    }

    [Fact]
    public Task GetEmployeeById_ForExistingEmployeeId_ShouldReturnBaseMessageDtoWithEmployeeObject()
    {
        return Task.CompletedTask;
    }
    [Fact]
    public Task GetEmployeeById_ForNonExistingEmployeeId_ShouldReturnBaseMessageDtoWithNullObject()
    {
        return Task.CompletedTask;
    }
    private async Task<Guid> SeedEmployee()
    {
        Employee employee = new Employee("Test", "Test", "test@test.pl", "Test123", Guid.NewGuid());
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory!.CreateScope();
        WgDbContext dbContext = scope.ServiceProvider.GetService<WgDbContext>()!;
        await dbContext.Employees.AddAsync(employee);
        await dbContext.SaveChangesAsync();
        return employee.Id;
    }

}