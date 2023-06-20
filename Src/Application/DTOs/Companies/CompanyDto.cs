using System;
namespace Application.DTOs.Companies
{
	public record CompanyDto
	{
		public string? Name { get; init; }
		public string CompanyLogo { get; init; } = string.Empty;
	}
}

