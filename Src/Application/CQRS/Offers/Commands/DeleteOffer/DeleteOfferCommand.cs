using Application.DTOs;
using MediatR;

namespace Application.CQRS.Offers.Commands.DeleteOffer;

public record DeleteOfferCommand : IRequest<BaseMessageDto>
{
    public Guid OfferId { get; init; }
}