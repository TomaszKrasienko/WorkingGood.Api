using Application.DTOs.EmployeesAuth;
using Domain.Models.Employee;
using FluentAssertions;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.IntegrationTests.Tests.Helpers;

namespace WebApi.IntegrationTests.Controllers.EmployeesAuthController;

[Collection("WebApiTests")]
public class EmployeeAuthControllerTests_ForgotPassword: IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    public EmployeeAuthControllerTests_ForgotPassword(WebApplicationFactory<Program> factory)
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
    public async Task ForgotPassword_ForValidResetPasswordDto_ShouldReturnOkResult()
    {
        //Arrange
            string email = $"{Guid.NewGuid()}@test.pl";
            string password = "Test123!";
            string newPassword = "Test123#";
            string resetToken = await SeedLoggedEmployee(email, password);
            ResetPasswordDto resetPasswordDto = new()
            {
                ResetToken = resetToken,
                NewPassword = newPassword,
                ConfirmNewPassword = newPassword
            };
            var httpContent = resetPasswordDto.ToJsonContent();
        //Act
            var response = await _client.PostAsync("api/EmployeesAuth/ResetPassword", httpContent);
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
        employee.SetResetToken();
        await dbContext.SaveChangesAsync();
        return employee.ResetToken?.Token ?? string.Empty;
    }
}