using System;
using Application.DTOs.Companies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.CQRS.Companies.Commands
{
	public record AddCompanyCommand : IRequest<IActionResult>
	{
		public CompanyDto? CompanyDto { get; init; }
	}
}

