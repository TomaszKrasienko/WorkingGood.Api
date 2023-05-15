using Application.DTOs;
using Application.DTOs.Offers;
using MediatR;

namespace Application.CQRS.Offers.Queries.GetOffersList;

public record GetOffersListQuery : IRequest<BaseMessageDto>
{
    public GetOffersListRequestDto GetOffersListRequestDto { get; init; } = new();
    public Guid? EmployeeId { get; init; }
}