using System;
namespace Domain.Interfaces.Validation
{
	public interface IEmployeeChecker
	{
		bool IsEmployeeExists(string email);
		bool IsVerificationTokenExists(string verificationToken);
	}
}

