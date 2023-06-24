using Application.DTOs;
using MediatR;

namespace Application.CQRS.Companies.Queries.GetCompanyById;

public record GetCompanyByIdQuery : IRequest<BaseMessageDto>
{
    public Guid? CompanyId { get; set; }
}