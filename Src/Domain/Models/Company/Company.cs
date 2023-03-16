using System;
using Domain.Interfaces;

namespace Domain.Models.Company
{
	public class Company : IAggregateRoot
	{
		public Guid Id { get; private set; }
		public string Name { get; private set; }
		public IReadOnlyCollection<Employee> Employees => _employees;
		private List<Employee> _employees;
		public Company()
		{

		}
	}
}

