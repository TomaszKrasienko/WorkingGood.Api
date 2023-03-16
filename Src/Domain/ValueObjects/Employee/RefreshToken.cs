using System;
namespace Domain.ValueObjects
{
	public class RefreshToken
	{
        public string? Token { get; private set; }
        public DateTime? CreatedAt { get; private set; }
        public DateTime? Expiration { get; private set; }
    }
}

