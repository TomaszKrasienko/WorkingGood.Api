using System;
using Application.DTOs;
using MediatR;

namespace Application.CQRS.Offers.Queries.GetById
{
	public record GetActiveOfferByIdQuery : IRequest<BaseMessageDto>
	{
		public Guid Id { get; init; }
	}
}

