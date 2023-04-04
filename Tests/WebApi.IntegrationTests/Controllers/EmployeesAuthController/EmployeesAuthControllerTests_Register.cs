using Application.DTOs.EmployeesAuth;
using Domain.Models.Company;
using FluentAssertions;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.IntegrationTests.Tests.Helpers;
using WebApi.IntegrationTests.Tests.Helpers.EmployeesAuthController;

namespace WebApi.IntegrationTests.Controllers.EmployeesAuthController;

[Collection("WebApiTests")]
public class EmployeesAuthControllerTests_Register : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    public EmployeesAuthControllerTests_Register(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<WgDbContext>));
                    services.Remove(dbContextOptions!);
                    services.AddDbContext<WgDbContext>(options => options.UseInMemoryDatabase("WorkingGoodTests"));
                    services.ConfigureTestRabbitMq();
                });
                
            });
        _client = _factory.CreateClient();
    }
    [Fact]
    public async Task Register_ForValidRegisterEmployeeDto_ShouldReturnOkResult()
    {
        //Arrange
            RegisterEmployeeDto registerEmployeeDto = new()
            {
                Email = $"{Guid.NewGuid().ToString()}@test.pl",
                Password = "Test123!",
                FirstName = "TestFirstName",
                LastName = "TestLastName"
            };
            var httpContent = registerEmployeeDto.ToJsonContent();
            var addedCompanyId = await SeedCompany();
        //Act
            var response = await _client.PostAsync($"api/EmployeesAuth/RegisterEmployee/{addedCompanyId}", httpContent);
        //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }  
    private async Task<Guid> SeedCompany()
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory!.CreateScope();
        WgDbContext dbContext = scope.ServiceProvider.GetService<WgDbContext>()!;
        Company company = new Company("TestCompany");
        await dbContext.Companies.AddAsync(company);
        await dbContext.SaveChangesAsync();
        return company.Id;
    }
    [Fact]
    public async Task Register_ForNonExistingCompany_ShouldReturnBadRequest()
    {
        //Arrange
            RegisterEmployeeDto registerEmployeeDto = new()
            {
                Email = "test@test.pl",
                Password = "Test123!",
                FirstName = "TestFirstName",
                LastName = "TestLastName"
            };
            var httpContent = registerEmployeeDto.ToJsonContent();
        //Act
            var response = await _client.PostAsync($"api/EmployeesAuth/RegisterEmployee/{Guid.NewGuid()}", httpContent);
        //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
    [Theory]
    [ClassData(typeof(InvalidRegisterEmployeeDtoProvider))]
    public async Task Register_ForNonValidRegisterEmployeeDto_ShouldReturnBadRequest(RegisterEmployeeDto registerEmployeeDto)
    {
        //Arrange
            var httpContent = registerEmployeeDto.ToJsonContent();
            var addedCompanyId = await SeedCompany();
        //Act
            var response = await _client.PostAsync($"api/EmployeesAuth/RegisterEmployee/{addedCompanyId}", httpContent);
        //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}  