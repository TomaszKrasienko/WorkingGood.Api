using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.IntegrationTests.Tests.Helpers;

public class FakeUserFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        //Todo: Przeanalizować
        var claimsPrincipal = new ClaimsPrincipal();
        claimsPrincipal.AddIdentity(new ClaimsIdentity(
            new[]
            {
                new Claim("EmployeeId", Guid.NewGuid().ToString())
            })
        );
        context.HttpContext.User = claimsPrincipal;
        await next();
    }
}