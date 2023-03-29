using Application.DTOs.Companies;
using Domain.Models.Company;
using Domain.Models.Employee;
using FluentAssertions;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.IntegrationTests.Tests.Helpers;

namespace WebApi.IntegrationTests.Controllers;

public class CompaniesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    public CompaniesControllerTests(WebApplicationFactory<Program> factory)
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
    public async Task AddCompany_ForNonExistingCompanyNameInCompanyDto_ShouldReturnOkStatus()
    {
        //Arrange
            CompanyDto companyDto = new()
            {
                Name = "TestCompany"
            };
            var httpContent = companyDto.ToJsonContent();
        //Act
            var response = await _client.PostAsync("api/Companies/AddCompany", httpContent);
        //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddCompany_ForExistingCompanyNameInCompanyDto_ShouldReturnBadRequest()
    {
        //Arrange
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            WgDbContext dbContext = scope.ServiceProvider.GetService<WgDbContext>();
            await dbContext.Companies.AddAsync(new Company("TestCompany"));
            await dbContext.SaveChangesAsync();
            CompanyDto companyDto = new()
            {
                Name = "TestCompany"
            };
            var httpContent = companyDto.ToJsonContent();
        //Act
            var response = await _client.PostAsync("api/Companies/AddCompany", httpContent);
        //Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}