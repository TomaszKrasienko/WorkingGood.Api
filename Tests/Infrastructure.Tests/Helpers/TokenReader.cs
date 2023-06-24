using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.Tests.Helpers;

public static class TokenReader
{
    private const string EMPLOYEE_ID_KEY = "EmployeeId";
    private const string EMPLOYEE_ROLES_KEY = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    private const string EMPLOYEE_EMAIL_KEY = "Email";
    public static Guid GetEmployeeIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        var id = jwtSecurityToken.Claims.First(claim => claim.Type == EMPLOYEE_ID_KEY).Value;
        return Guid.Parse(id);
    }
    public static List<string> GetRolesFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        var roles = jwtSecurityToken.Claims.ToList().Where(x => x.Type == EMPLOYEE_ROLES_KEY);
        return roles.Select(x => x.Value).ToList();
    }
    public static string GetEmployeeEmailFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        var email = jwtSecurityToken.Claims.First(claim => claim.Type == EMPLOYEE_EMAIL_KEY).Value;
        return email;
    }
}