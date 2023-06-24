using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Common.Exceptions;
using Domain.Interfaces;
using Domain.Services;
using Domain.ValueObjects;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Models.Employee
{
	public class Employee : AggregateRoot<Guid>
	{
		public EmployeeName EmployeeName { get; private set; }	
        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public RefreshToken RefreshToken { get; private set; }
        public EmployeeStatus EmployeeStatus { get; private set; }
        public VerificationToken VerificationToken { get; private set; }
        public ResetToken? ResetToken { get; private set; }
        public Guid CompanyId { get; private set; }

        public Employee() : base(Guid.NewGuid())
        {
	        
        }
        public Employee(string firstName, string lastName, string email, string password, Guid companyId) : base(Guid.NewGuid())
        {
	        EmployeeName = new EmployeeName(firstName, lastName);
            Email = new Email(email);
            Password = new Password(password);
            VerificationToken = new VerificationToken();
            CompanyId = companyId;
            EmployeeStatus = new EmployeeStatus();
        }
        public void Activate()
        {
	        EmployeeStatus.ChangeStatus();
	        VerificationToken.ConfirmToken();
        }
        public LoginToken Login(string password, ITokenProvider tokenProvider)
        {
	        if (!EmployeeStatus.IsActive)
		        throw new LoginException("Employee is not active");
	        if (!Password.IsPasswordCorrect(password))
		        throw new LoginException("Password is incorrect");
	        GenerateRefreshToken();
	        return tokenProvider.Provide(
		        emailAddress: Email.EmailAddress,
		        roles: new List<string>(){"user"},
		        employeeId: Id,
		        companyId: CompanyId);
        }
        public bool IsPasswordMatch(string password)
        {
	        return Password.IsPasswordCorrect(password);
        }
        public LoginToken Refresh(ITokenProvider tokenProvider)
        {
	        GenerateRefreshToken();
	        return tokenProvider.Provide(		        
		        emailAddress: Email.EmailAddress,
		        roles: new List<string>(){"user"},
		        employeeId: Id,
		        companyId: CompanyId);
        }
        private void GenerateRefreshToken()
        {
	        RefreshToken = new RefreshToken();
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

