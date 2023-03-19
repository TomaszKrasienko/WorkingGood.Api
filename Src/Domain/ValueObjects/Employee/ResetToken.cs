using System;
namespace Domain.ValueObjects
{
	public class ResetToken
	{
        public string? Token { get; private set; }
        public DateTime? ExpirationDate { get; private set; }

        public ResetToken()
        {
	        
        }
    }
}

