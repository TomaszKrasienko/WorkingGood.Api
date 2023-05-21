using Application.Common.Extensions.Validation;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Models.Offer;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Offers.Commands.ChangeOfferStatus;

public class ChangeOfferStatusCommandHandler : IRequestHandler<ChangeOfferStatusCommand,BaseMessageDto>
{
    private readonly ILogger<ChangeOfferStatusCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<ChangeOfferStatusCommand> _validator;
    public ChangeOfferStatusCommandHandler(ILogger<ChangeOfferStatusCommandHandler> logger, IUnitOfWork unitOfWork, IValidator<ChangeOfferStatusCommand> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<BaseMessageDto> Handle(ChangeOfferStatusCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(validationResult.Errors.GetErrorString());
            return new BaseMessageDto
            {
                Errors = validationResult.Errors.GetErrorsStringList()
            };
        }
        Offer offer = await _unitOfWork.OffersRepository.GetByIdAsync(request.OfferId);
        offer.ChangeStatus();
        await _unitOfWork.CompleteAsync();
        return new BaseMessageDto
        {
            Message = "Status has been changed"
        };
    }
}