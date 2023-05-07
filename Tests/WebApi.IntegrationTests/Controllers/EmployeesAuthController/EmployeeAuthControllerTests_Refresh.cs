using Application.DTOs.EmployeesAuth;
using Domain.Models.Employee;
using Domain.Services;
using FluentAssertions;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.IntegrationTests.Tests.Helpers;

namespace WebApi.IntegrationTests.Controllers.EmployeesAuthController;

[Collection("WebApiTests")]
public class EmployeeAuthControllerTests_Refresh : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    public EmployeeAuthControllerTests_Refresh(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<WgDbContext>));
                    services.Remove(dbContextOptions!);
                    services.AddDbContext<WgDbContext>(options => options.UseInMemoryDatabase("WorkingGoodTests"));
                });
            });
        _client = _factory.CreateClient();
    }
    [Fact]
    public async Task Refresh_ForLoggedUser_ShouldReturnOkResult()
    {
        //Arrange
            string refreshToken = await SeedLoggedEmployee("test@test.pl", "Test123!");
            RefreshDto refreshDto = new()
            {
                RefreshToken = refreshToken
            };
            var httpContent = refreshDto.ToJsonContent();
        //Act
            var response = await _client.PostAsync($"employeesAuth/refresh", httpContent);
        //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
    private async Task<string> SeedLoggedEmployee(string email, string password)
    {
        Employee employee = new Employee("Test", "Test", email, password, Guid.NewGuid());
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory!.CreateScope();
        WgDbContext dbContext = scope.ServiceProvider.GetService<WgDbContext>()!;
        await dbContext.Employees.AddAsync(employee);
        await dbContext.SaveChangesAsync();
        employee.Activate();
        await dbContext.SaveChangesAsync();
        ITokenProvider tokenProvider = scope.ServiceProvider.GetRequiredService<ITokenProvider>();
        var loginResponse = employee.Login(password, tokenProvider);
        await dbContext.SaveChangesAsync();
        return employee.RefreshToken?.Token ?? string.Empty;
    }
    [Fact]
    public async Task Refresh_ForNonLoggedUser_ShouldReturnOkResult()
    {
        //Arrange
            string refreshToken = await SeedNotLoggedEmployee("test@test.pl", "Test123!");
            RefreshDto refreshDto = new()
            {
                RefreshToken = refreshToken
            };
            var httpContent = refreshDto.ToJsonContent();
        //Act
            var response = await _client.PostAsync($"/employeesAuth/refresh", httpContent);
        //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }
    private async Task<string> SeedNotLoggedEmployee(string email, string password)
    {
        Employee employee = new Employee("Test", "Test", email, password, Guid.NewGuid());
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory!.CreateScope();
        WgDbContext dbContext = scope.ServiceProvider.GetService<WgDbContext>()!;
        await dbContext.Employees.AddAsync(employee);
        await dbContext.SaveChangesAsync();
        employee.Activate();
        await dbContext.SaveChangesAsync();
        return employee.RefreshToken?.Token ?? string.Empty;
    }
}