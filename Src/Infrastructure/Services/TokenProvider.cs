using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Services;
using Domain.ValueObjects;
using Infrastructure.Common.ConfigModels;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class TokenProvider : ITokenProvider
{
    private readonly JwtConfig _jwtConfig;
    public TokenProvider(JwtConfig jwtConfig)
    {
        _jwtConfig = jwtConfig;
    }
    public LoginToken Provide(string emailAddress, List<string> roles, string userId)
    {	        
        //Todo: do zmiany
        DateTime expiration = DateTime.Now.AddMinutes(20);
        List<Claim> claims = new()
        {
            new Claim("Email", emailAddress),
            new(ClaimTypes.Role, roles.FirstOrDefault()),
            new Claim("EmployeeId", userId)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.TokenKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: expiration,
            signingCredentials: creds,
            audience: _jwtConfig.Audience,
            issuer: _jwtConfig.Issuer
        );
        return new LoginToken
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration
        };
    }
}