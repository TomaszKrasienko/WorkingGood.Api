using System;
using Application.Common.Extensions.Validation;
using Application.DTOs;
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
	        _logger.LogInformation("Handling GetByIdQuery");
	        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
	        if (!validationResult.IsValid)
	        {
		        _logger.LogWarning(validationResult.Errors.GetErrorString());
		        return new()
		        {
			        Errors = validationResult.Errors.GetErrorString()
		        };
	        }
	        var entity = await _unitOfWork.OffersRepository.GetByIdAsync(request.Id);
			return new()
			{
				Object = entity
			};
        }
    }
}

