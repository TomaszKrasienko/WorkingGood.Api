using System;
using System.Security.Cryptography;

namespace Domain.ValueObjects
{
	public class RefreshToken
	{
        public string? Token { get; private set; }
        public DateTime? CreatedAt { get; private set; }
        public DateTime? Expiration { get; private set; }
        public RefreshToken()
        {
            Generate();
        }
        private void Generate()
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)).Replace("/", "");
            CreatedAt = DateTime.Now;
            Expiration = DateTime.Now.AddDays(1);
        }
    }
}

