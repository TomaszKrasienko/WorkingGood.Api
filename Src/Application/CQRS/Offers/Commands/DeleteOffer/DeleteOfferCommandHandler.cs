using Application.Common.Extensions.Validation;
using Application.DTOs;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Communication;
using Domain.Models.Offer;
using FluentValidation;
using Infrastructure.Communication.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using WorkingGood.Log;

namespace Application.CQRS.Offers.Commands.DeleteOffer;

public sealed class DeleteOfferCommandHandler : IRequestHandler<DeleteOfferCommand, BaseMessageDto>
{
    private readonly IWgLog<DeleteOfferCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<DeleteOfferCommand> _validator;
    private readonly IBrokerSender _brokerSender;
    public DeleteOfferCommandHandler(
        IWgLog<DeleteOfferCommandHandler> logger, 
        IUnitOfWork unitOfWork, 
        IValidator<DeleteOfferCommand> validator,
        IBrokerSender brokerSender)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _brokerSender = brokerSender;
    }
    public async Task<BaseMessageDto> Handle(DeleteOfferCommand request, CancellationToken cancellationToken)
    {
        _logger.Info("Handling DeleteOfferCommandHandler");
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.Warn(validationResult.Errors.GetErrorString());
            return new BaseMessageDto()
            {
                Errors = validationResult.Errors.GetErrorsStringList()
            };
        }
        Offer offer = await _unitOfWork.OffersRepository.GetByIdAsync(request.OfferId);
        await _unitOfWork.OffersRepository.Delete(offer);
        _brokerSender.Send(MessageDestinations.RemoveApplication, new RemoveApplication
        {
            OfferId = request.OfferId
        });
        await _unitOfWork.CompleteAsync();
        return new BaseMessageDto()
        {
            Message = "Offer has been deleted"
        };
    }
}