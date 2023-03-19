using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Common.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Models.Employee
{
	public class Employee : IAggregateRoot
	{
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public Password Password { get; private set; }
        public RefreshToken RefreshToken { get; private set; }
        public bool IsActive { get; private set; }
        public VerificationToken VerificationToken { get; private set; }
        public ResetToken? ResetToken { get; private set; }
        public Guid CompanyId { get; private set; }
        public Employee()
        {

        }
        public Employee(string firstName, string lastName, string email, string password, Guid companyId)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = new Password(password);
            VerificationToken = new VerificationToken();
            CompanyId = companyId;
            IsActive = false;
        }
        public void Activate()
        {
	        IsActive = true;
	        VerificationToken.ConfirmToken();
        }
        public LoginToken Login(string password, string tokenKey, string audience, string issuer)
        {
	        if (!IsActive)
		        throw new LoginException("Employee is not active");
	        if (!Password.IsPasswordCorrect(password))
		        throw new LoginException("Password is incorrect");
	        GenerateRefreshToken();
	        return GetToken(tokenKey, audience, issuer);
        }
        public bool IsPasswordMatch(string password)
        {
	        return Password.IsPasswordCorrect(password);
        }
        public LoginToken Refresh(string tokenKey, string audience, string issuer)
        {
	        if (!RefreshToken.IsValid())
		        throw new LoginException("Refresh token is invalid");
	        GenerateRefreshToken();
	        return GetToken(tokenKey, audience, issuer);
        }
        private void GenerateRefreshToken()
        {
	        RefreshToken = new RefreshToken();
        }
        private LoginToken GetToken(string tokenKey, string audience, string issuer)
        {
	        DateTime expiration = DateTime.Now.AddMinutes(20);
	        List<Claim> claims = new()
	        {
		        new(ClaimTypes.Name, Email),
		        new(ClaimTypes.Role, "User"),
		        new Claim("EmployeeId", Id.ToString())
	        };
	        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
	        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
	        var token = new JwtSecurityToken(
		        claims: claims,
		        expires: expiration,
		        signingCredentials: creds,
		        audience: audience,
		        issuer: issuer
	        );
	        return new LoginToken
	        {
		        Token = new JwtSecurityTokenHandler().WriteToken(token),
		        Expiration = expiration
	        };
        }
        public void SetNewPassword(string password)
        {
	        Password = new Password(password);
        }
        public void SetResetToken()
        {
	        ResetToken = new ResetToken();
        }
	}
}

