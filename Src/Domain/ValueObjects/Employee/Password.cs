using System;
namespace Domain.ValueObjects
{
	public class Password
	{
		public byte[] Salt { get; private set; }
		public byte[] Hash { get; private set; }
	}
}
	


