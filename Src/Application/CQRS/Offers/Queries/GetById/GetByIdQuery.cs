using System;
using Application.DTOs;
using MediatR;

namespace Application.CQRS.Offers.Queries.GetById
{
	public record GetByIdQuery : IRequest<BaseMessageDto>
	{
		public Guid Id { get; init; }
	}
}

