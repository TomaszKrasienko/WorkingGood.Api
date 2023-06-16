using Application.DTOs;
using MediatR;

namespace Application.CQRS.Offers.Queries.GetPositions;

public record GetPositionsListQuery : IRequest<BaseMessageDto> { }