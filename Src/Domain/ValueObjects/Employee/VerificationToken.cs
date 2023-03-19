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
        private void Generate()
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)).Replace("/", "");
        }
        internal void ConfirmToken()
        {
            ConfirmDate = DateTime.Now;
        }
    }
}

