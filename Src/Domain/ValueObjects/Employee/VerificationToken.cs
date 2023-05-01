using System;
using System.Security.Cryptography;

namespace Domain.ValueObjects
{
	public class VerificationToken : ValueObject
	{
        public string Token { get; private set; }
        public DateTime? ConfirmDate { get; private set; }
        public VerificationToken()
        {
            Generate();
        }
        private void Generate()
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)).Replace("/", "").Replace("==", "");
        }
        internal void ConfirmToken()
        {
            ConfirmDate = DateTime.Now;
        }
        public override IEnumerable<object> GetAtomicValue()
        {
            yield return Token;
            yield return ConfirmDate ?? null!;
        }
    }
}

