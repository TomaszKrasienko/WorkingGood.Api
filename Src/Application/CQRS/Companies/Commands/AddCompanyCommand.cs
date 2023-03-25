using System;
using Application.DTOs;
using Application.DTOs.Companies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.CQRS.Companies.Commands
{
	public record AddCompanyCommand : IRequest<BaseMessageDto>
	{
		public CompanyDto? CompanyDto { get; init; }
	}
}

