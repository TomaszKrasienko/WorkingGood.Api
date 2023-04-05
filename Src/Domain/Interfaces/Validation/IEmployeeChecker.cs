using System;
namespace Domain.Interfaces.Validation
{
	public interface IEmployeeChecker
	{
		bool IsEmployeeExists(string email);
		bool IsEmployeeExists(Guid employeeId);
		bool IsVerificationTokenExists(string verificationToken);
		bool IsResetTokenExists(string resetToken);
	}
}

