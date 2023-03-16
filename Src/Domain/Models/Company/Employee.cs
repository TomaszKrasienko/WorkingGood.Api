using System;
using Domain.ValueObjects;

namespace Domain.Models.Company
{
	public class Employee
	{
        public Guid Id { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public Password Password { get; private set; }
        public RefreshToken RefreshToken { get; private set; }
        public bool IsActive { get; private set; }
        public VerificationToken VerificationToken { get; private set; }
        public ResetToken ResetToken { get; private set; }
        public Employee(string userName, string email, string password)
		{
            Id = Guid.NewGuid();
            UserName = userName;
            Email = email;
		}
	}
}

