using Application.DTOs;
using MediatR;

namespace Application.CQRS.Offers.Queries.GetActiveOffers;

public record GetActiveOffersQuery : IRequest<BaseMessageDto>
{
    
}