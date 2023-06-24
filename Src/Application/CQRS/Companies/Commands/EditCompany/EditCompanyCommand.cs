using Application.DTOs;
using Application.DTOs.Companies;
using MediatR;

namespace Application.CQRS.Companies.Commands.EditCompany;

public record EditCompanyCommand : IRequest<BaseMessageDto>
{
    public Guid? CompanyId { get; init; }
    public CompanyDto CompanyDto { get; init; } = new();
}