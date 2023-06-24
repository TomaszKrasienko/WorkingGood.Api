using Application.DTOs;
using MediatR;

namespace Application.CQRS.Companies.Queries.GetCompanyByOfferId;

public record GetCompanyByOfferIdQuery : IRequest<BaseMessageDto>
{
    public Guid OfferId { get; init; }
}