using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.IntegrationTests.Tests.Helpers;

public class FakeUserFilter : IAsyncActionFilter
{
    private readonly string? _employeeId;
    public FakeUserFilter()
    {
        
    }
    public FakeUserFilter(Guid employeeId)
    {
        _employeeId = employeeId.ToString();
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //Todo: Przeanalizować
        var claimsPrincipal = new ClaimsPrincipal();
        claimsPrincipal.AddIdentity(new ClaimsIdentity(
            new[]
            {
                new Claim("EmployeeId",  _employeeId ?? Guid.NewGuid().ToString())
            })
        );
        context.HttpContext.User = claimsPrincipal;
        await next();
    }
}