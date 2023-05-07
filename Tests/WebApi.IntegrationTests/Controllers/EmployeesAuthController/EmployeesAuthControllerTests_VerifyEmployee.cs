using Domain.Models.Employee;
using FluentAssertions;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.IntegrationTests.Tests.Helpers;

namespace WebApi.IntegrationTests.Controllers.EmployeesAuthController;

[Collection("WebApiTests")]
public class EmployeesAuthControllerTests_VerifyEmployee : IClassFixture<WebApplicationFactory<Program>>
{    
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    public EmployeesAuthControllerTests_VerifyEmployee(WebApplicationFactory<Program> factory)
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
    public async Task VerifyEmployee_ForExistingVerificationToken_ShouldReturnOkResult()
    {
        //Arrange
        string httpContentString = string.Empty;
        var httpContent = httpContentString.ToJsonContent();
        var verificationToken = await SeedEmployeeAndGetVerificationToken();
        //Act
        var response = await _client.PostAsync($"employeesAuth/VerifyEmployee/{verificationToken}", httpContent);
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
    private async Task<string> SeedEmployeeAndGetVerificationToken()
    {
        Employee employee = new Employee("Test", "Test", "test@test.pl", "Test123", Guid.NewGuid());
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory!.CreateScope();
        WgDbContext dbContext = scope.ServiceProvider.GetService<WgDbContext>()!;
        await dbContext.Employees.AddAsync(employee);
        await dbContext.SaveChangesAsync();
        return employee.VerificationToken.Token;
    }
    [Fact]
    public async Task VerifyEmployee_ForNonExistingVerificationToken_ShouldReturnBadRequestResult()
    {
        //Arrange
        string httpContentString = string.Empty;
        var httpContent = httpContentString.ToJsonContent();
        //Act
        var response = await _client.PostAsync($"employeesAuth/VerifyEmployee/Test", httpContent);
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }    
}