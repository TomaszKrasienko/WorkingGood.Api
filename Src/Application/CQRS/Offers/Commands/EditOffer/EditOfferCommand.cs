using Application.DTOs;
using Application.DTOs.Offers;
using MediatR;

namespace Application.CQRS.Offers.Commands.EditOffer;

public record EditOfferCommand : IRequest<BaseMessageDto>
{
    public EditOfferRequestDto OfferDto { get; init; } = new();
    public Guid OfferId { get; init; }
}