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
                .Any(x => x.Email.EmailAddress == email);
        }
        public bool IsEmployeeExists(Guid employeeId)
        {
	        return _context
		        .Employees
		        .Any(x => x.Id == employeeId);
        }
        public bool IsVerificationTokenExists(string verificationToken)
        {
	        return _context
		        .Employees
		        .Any(x => x.VerificationToken.Token == verificationToken);
        }
        public bool IsResetTokenExists(string resetToken)
        {
	        return _context
		        .Employees
		        .Any(x => x.ResetToken!.Token == resetToken);
        }
	}
}

