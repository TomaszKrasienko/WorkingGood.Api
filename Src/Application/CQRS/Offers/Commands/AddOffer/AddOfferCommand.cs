using Application.DTOs;
using Application.DTOs.Offers;
using MediatR;

namespace Application.CQRS.Offers.Commands;

public class AddOfferCommand : IRequest<BaseMessageDto>
{
    public OfferDto OfferDto { get; set; }
    public Guid EmployeeId { get; set; }
}