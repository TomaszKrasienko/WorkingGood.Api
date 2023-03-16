using System;
using System.Security.Cryptography;

namespace Domain.ValueObjects
{
	public class VerificationToken
	{
        public string Token { get; private set; }
        public DateTime? ConfirmDate { get; private set; }
        public VerificationToken()
        {
            Generate();
        }
        private string Generate()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)).Replace("/", "");
        }
    }
}

