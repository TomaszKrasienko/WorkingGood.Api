using Application.DTOs;
using MediatR;

namespace Application.CQRS.Offers.Queries.GetOfferById;

public record GetOfferByIdCommand : IRequest<BaseMessageDto>
{
    public Guid OfferId { get; init; }
}