﻿using System;
using Domain.Interfaces;
using Domain.ValueObjects.Company;

namespace Domain.Models.Company
{
	public sealed class Company : AggregateRoot<Guid>
	{
		public CompanyName Name { get; private set; }
		public Company() : base(Guid.NewGuid())
		{
			
		}
		public Company(string name) : base(Guid.NewGuid())
		{
			Name = new CompanyName(name);
		}
	}
}

