using System;
namespace Domain.ValueObjects
{
	public class VerificationToken
	{
        public string Token { get; private set; }
        public DateTime? ConfirmDate { get; private set; }
    }
}

