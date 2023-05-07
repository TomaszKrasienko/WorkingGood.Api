using System;
using Application.Common.Extensions.Validation;
using Application.DTOs;
using Application.ViewModels.GetEmployee;
using Application.ViewModels.Offer;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Offers.Queries.GetById
{
	public class GetActiveOfferByIdQueryHandler : IRequestHandler<GetActiveOfferByIdQuery, BaseMessageDto>
	{
		private readonly ILogger<GetActiveOfferByIdQueryHandler> _logger;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IValidator<GetActiveOfferByIdQuery> _validator;
		private readonly IMapper _mapper;
		public GetActiveOfferByIdQueryHandler(
			ILogger<GetActiveOfferByIdQueryHandler> logger, 
			IUnitOfWork unitOfWork, 
			IValidator<GetActiveOfferByIdQuery> validator,
			IMapper mapper)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
			_validator = validator;
			_mapper = mapper;
		}
        public async Task<BaseMessageDto> Handle(GetActiveOfferByIdQuery request, CancellationToken cancellationToken)
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
	        GetOfferVM getEmployeeVm = _mapper.Map<GetOfferVM>(entity);
			return new()
			{
				Object = getEmployeeVm
			};
        }
    }
}

