using Application.DTOs;
using MediatR;

namespace Application.CQRS.Offers.Queries.GetOfferStatus;

public record GetStatusQuery : IRequest<BaseMessageDto>
{
    public Guid OfferId { get; init; }
}