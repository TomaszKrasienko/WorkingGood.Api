using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace WebApi.IntegrationTests.Tests.Helpers;

public class FakePolicyEvaluator : IPolicyEvaluator
{
    private static string? _employeeId;
    public FakePolicyEvaluator()
    {
        
    }
    public FakePolicyEvaluator(Guid employeeId)
    {
        _employeeId = employeeId.ToString();
    }
    public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
    {
        //Todo: Przeanalizować
        var claimsPrincipal = new ClaimsPrincipal();
        claimsPrincipal.AddIdentity(new ClaimsIdentity(
            new[]
            {
                new Claim("EmployeeId",  _employeeId ?? Guid.NewGuid().ToString())
            })
        );
        var ticket = new AuthenticationTicket(claimsPrincipal, "Test");
        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }

    public Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy, AuthenticateResult authenticationResult, HttpContext context,
        object? resource)
    {        
        var result = PolicyAuthorizationResult.Success();
        return Task.FromResult(result);
    }
}