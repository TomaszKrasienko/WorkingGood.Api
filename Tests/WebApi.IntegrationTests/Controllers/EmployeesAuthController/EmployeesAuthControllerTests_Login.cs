using Application.DTOs.EmployeesAuth;
using Domain.Models.Employee;
using FluentAssertions;
using Infrastructure.Common.ConfigModels;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using WebApi.IntegrationTests.Tests.Helpers;

namespace WebApi.IntegrationTests.Controllers.EmployeesAuthController;

public class EmployeesAuthControllerTests_Login: IClassFixture<WebApplicationFactory<Program>>
{    
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    public EmployeesAuthControllerTests_Login(WebApplicationFactory<Program> factory)
    {
        JwtConfig jwtConfig = new()
        {
            TokenKey = "my top secret key",
            Audience = "test",
            Issuer = "test"
        };
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<WgDbContext>));
                    services.Remove(dbContextOptions!);
                    services.AddDbContext<WgDbContext>(options => options.UseInMemoryDatabase("WorkingGoodTests"));
                    services.AddSingleton(jwtConfig);
                });
            });
        _client = _factory.CreateClient();
    }
    [Fact]
    public async Task Login_ForValidCredentialsDto_ShouldReturnOkResult()
    {
        string email = "test@test.pl";
        string password = "Test123!";
        //Arrange
        CredentialsDto credentials = new()
        {
            Email = email,
            Password = password,
        };
        var httpContent = credentials.ToJsonContent();
        await SeedEmployee(email, password);
        //Act
        var response = await _client.PostAsync($"api/EmployeesAuth/Login", httpContent);
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_ForInvalidPassword_ShouldReturnBadRequest()
    {
        string email = "test@test.pl";
        string validPassword = "Test123!";
        string invalidPassword = "InvalidPassword123!";
        //Arrange
        CredentialsDto credentials = new()
        {
            Email = email,
            Password = validPassword,
        };
        var httpContent = credentials.ToJsonContent();
        await SeedEmployee(email, invalidPassword);
        //Act
        var response = await _client.PostAsync($"api/EmployeesAuth/Login", httpContent);
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
    [Fact]
    public async Task Login_ForNotExistingEmployee_ShouldReturnBadRequest()
    {
        string email = "test@test.pl";
        string password = "Test123!";
        //Arrange
        CredentialsDto credentials = new()
        {
            Email = email,
            Password = password,
        };
        var httpContent = credentials.ToJsonContent();
        //Act
        var response = await _client.PostAsync($"api/EmployeesAuth/Login", httpContent);
        //Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
    private async Task SeedEmployee(string email, string password)
    {
        Employee employee = new Employee("Test", "Test", email, password, Guid.NewGuid());
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory!.CreateScope();
        WgDbContext dbContext = scope.ServiceProvider.GetService<WgDbContext>()!;
        await dbContext.Employees.AddAsync(employee);
        await dbContext.SaveChangesAsync();
        employee.Activate();
        await dbContext.SaveChangesAsync();
    }
}