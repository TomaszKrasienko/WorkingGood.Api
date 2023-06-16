using Application.Common.Extensions.Validation;
using Application.CQRS.Offers.Queries.GetOfferStatus;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Models.Offer;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using WorkingGood.Log;

namespace Application.CQRS.Offers.Queries.GetStatus;

public class GetStatusQueryHandler : IRequestHandler<GetStatusQuery, BaseMessageDto>
{
    private readonly IWgLog<GetStatusQuery> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<GetStatusQuery> _validator;
    public GetStatusQueryHandler(IWgLog<GetStatusQuery> logger, IUnitOfWork unitOfWork, IValidator<GetStatusQuery> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<BaseMessageDto> Handle(GetStatusQuery request, CancellationToken cancellationToken)
    {
        _logger.Info("Handling GetStatusQuery");
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.Warn(validationResult.Errors.GetErrorString());
            return new BaseMessageDto
            {
                Errors = validationResult.Errors.GetErrorString()
            };
        }
        Offer offer = await _unitOfWork.OffersRepository.GetByIdAsync(request.OfferId);
        return new BaseMessageDto()
        {
            Object = offer.OfferStatus.IsActive
        };
    }
}