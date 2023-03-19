using System;
using System.Security.Cryptography;
using System.Text;

namespace Domain.ValueObjects
{
	public class Password
	{
		public byte[] Salt { get; private set; }
		public byte[] Hash { get; private set; }
		public Password(byte[] salt, byte[] hash)
		{
			Salt = salt;
			Hash = hash;
		}
		public Password(string password)
		{
			GeneratePassword(password);
		}
		private void GeneratePassword(string password)
		{
			using (var hmac = new HMACSHA512())
			{
				Salt = hmac.Key;
				Hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
			}
		}        
		internal bool IsPasswordCorrect(string password)
		{
			using (var hmac = new HMACSHA512(Salt))
			{
				var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
				var result = computedHash.SequenceEqual(Hash);
				return result;
			}
		}
	}
}
	


