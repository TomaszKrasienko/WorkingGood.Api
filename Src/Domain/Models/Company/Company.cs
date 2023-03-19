using System;
using Domain.Interfaces;

namespace Domain.Models.Company
{
	public class Company : IAggregateRoot
	{
		public Guid Id { get; private set; }
		public string Name { get; private set; }
		public Company() { }
		public Company(string name)
		{
			Id = Guid.NewGuid();
			Name = name;
		}
	}
}

