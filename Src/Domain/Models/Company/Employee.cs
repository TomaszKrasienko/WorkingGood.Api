using System;
using Domain.ValueObjects;

namespace Domain.Models.Company
{
	public class Employee
	{
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public Password Password { get; private set; }
        public RefreshToken RefreshToken { get; private set; }
        public bool IsActive { get; private set; }
        public VerificationToken VerificationToken { get; private set; }
        public ResetToken ResetToken { get; private set; }
        public Employee()
        {

        }
        public Employee(string firstName, string lastName, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = new Password(password);
            RefreshToken = new RefreshToken();
            VerificationToken = new VerificationToken();
            IsActive = false;
        }
	}
}

