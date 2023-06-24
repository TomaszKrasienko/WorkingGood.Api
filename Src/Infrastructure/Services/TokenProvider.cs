using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Enums;
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
    public LoginToken Provide(string emailAddress, List<string> roles, Guid employeeId, Guid companyId)
    {
        DateTime expiration = DateTime.Now.AddMinutes(20);
        List<Claim> claims = new()
        {
            new Claim(TokenKey.Email.ToString(), emailAddress),
            new(TokenKey.Roles.ToString(), roles.FirstOrDefault()),
            new Claim(TokenKey.EmployeeId.ToString(), employeeId.ToString()),
            new Claim(TokenKey.CompanyId.ToString(), companyId.ToString())
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