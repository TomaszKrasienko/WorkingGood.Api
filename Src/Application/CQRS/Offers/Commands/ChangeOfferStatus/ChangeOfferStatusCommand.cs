using Application.DTOs;
using MediatR;

namespace Application.CQRS.Offers.Commands.ChangeOfferStatus;

public record ChangeOfferStatusCommand : IRequest<BaseMessageDto>
{
    public Guid OfferId { get; init; }
}