using Application.DTOs;
using Application.DTOs.Offers;
using MediatR;

namespace Application.CQRS.Offers.Commands;

public record AddOfferCommand : IRequest<BaseMessageDto>
{
    public OfferDto OfferDto { get; init; } = new();
    public Guid? EmployeeId { get; init; }
}