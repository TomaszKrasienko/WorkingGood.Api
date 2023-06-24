using System;
namespace Application.DTOs.Companies
{
	public record CompanyDto
	{
		public string? Name { get; init; }
		public string? Logo { get; init; }
	}
}

