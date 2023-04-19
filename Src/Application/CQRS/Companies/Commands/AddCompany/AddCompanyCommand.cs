using Application.DTOs;
using Application.DTOs.Companies;
using MediatR;

namespace Application.CQRS.Companies.Commands.AddCompany
{
	public record AddCompanyCommand : IRequest<BaseMessageDto>
	{
		public CompanyDto? CompanyDto { get; init; }
	}
}

