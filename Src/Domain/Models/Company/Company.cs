using System;
using Domain.Interfaces;
using Domain.ValueObjects.Company;

namespace Domain.Models.Company
{
	public sealed class Company : AggregateRoot<Guid>
	{
		public CompanyName Name { get; private set; }
		public CompanyLogo? Logo { get; private set; }
		public Company() : base(Guid.NewGuid())
		{
			
		}
		public Company(string name, string? logo) : base(Guid.NewGuid())
		{
			Name = new CompanyName(name);
			if(logo is not null)
				Logo = new CompanyLogo(logo);
		}

		public void EditName(string name)
		{
			Name = new CompanyName(name);
		}

		public void EditLogo(string logo)
		{
			Logo = new CompanyLogo(logo);
		}
	}
}

