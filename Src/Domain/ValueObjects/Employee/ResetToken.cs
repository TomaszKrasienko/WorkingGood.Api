using System;
using System.Security.Cryptography;

namespace Domain.ValueObjects
{
	public class ResetToken
	{
        public string? Token { get; private set; }
        public DateTime? ExpirationDate { get; private set; }

        public ResetToken()
        {
	        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)).Replace("/", "");
	        ExpirationDate = DateTime.Now.AddDays(1);
        }
    }
}

