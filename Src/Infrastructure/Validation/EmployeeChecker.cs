using System;
using Domain.Interfaces.Validation;
using Infrastructure.Persistance;

namespace Infrastructure.Validation
{
	public class EmployeeChecker : IEmployeeChecker
	{
        private readonly WgDbContext _context;
		public EmployeeChecker(WgDbContext context)
		{
            _context = context;
		}
        public bool IsEmployeeExists(string email)
        {
            return _context
                .Employees
                .Any(x => x.Email == email);
        }

        public bool IsVerificationTokenExists(string verificationToken)
        {
	        return _context
		        .Employees
		        .Any(x => x.VerificationToken.Token == verificationToken);
        }
	}
}

