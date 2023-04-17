using System;
using Application.DTOs;
using Application.Extensions.Validation;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Offers.Queries.GetById
{
	public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, BaseMessageDto>
	{
		private readonly ILogger<GetByIdQueryHandler> _logger;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IValidator<GetByIdQuery> _validator;
		public GetByIdQueryHandler(ILogger<GetByIdQueryHandler> logger, IUnitOfWork unitOfWork, IValidator<GetByIdQuery> validator)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
			_validator = validator;
		}

        public async Task<BaseMessageDto> Handle(GetByIdQuery request, CancellationToken cancellationToken)
        {
			var validationResult = await _validator.ValidateAsync(request);
			if (!validationResult.IsValid)
				return new()
				{
					Errors = validationResult.Errors.GetErrorString()
				};
			var entity = await _unitOfWork.OffersRepository.GetByIdAsync(request.Id);
			return new()
			{
				Object = entity
			};
        }
    }
}

