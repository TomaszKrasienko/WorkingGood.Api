using Application.Common.Extensions.Validation;
using Application.CQRS.Offers.Queries.GetOfferStatus;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Models.Offer;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Offers.Queries.GetStatus;

public class GetStatusQueryHandler : IRequestHandler<GetStatusQuery, BaseMessageDto>
{
    private readonly ILogger<GetStatusQuery> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<GetStatusQuery> _validator;
    public GetStatusQueryHandler(ILogger<GetStatusQuery> logger, IUnitOfWork unitOfWork, IValidator<GetStatusQuery> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<BaseMessageDto> Handle(GetStatusQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetStatusQuery");
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(validationResult.Errors.GetErrorString());
            return new BaseMessageDto
            {
                Errors = validationResult.Errors.GetErrorString()
            };
        }
        Offer offer = await _unitOfWork.OffersRepository.GetByIdAsync(request.OfferId);
        return new BaseMessageDto()
        {
            Object = offer.IsActive
        };
    }
}