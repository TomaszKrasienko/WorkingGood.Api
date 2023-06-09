using Application.Common.Extensions.Validation;
using Application.DTOs;
using Application.ViewModels.Offer;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models.Offer;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using WorkingGood.Log;

namespace Application.CQRS.Offers.Commands.EditOffer;

public class EditOfferCommandHandler : IRequestHandler<EditOfferCommand, BaseMessageDto>
{
    private readonly IWgLog<EditOfferCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<EditOfferCommand> _validator;
    private readonly IMapper _mapper;
    public EditOfferCommandHandler(
        IWgLog<EditOfferCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IValidator<EditOfferCommand> validator,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _mapper = mapper;
    }
    public async Task<BaseMessageDto> Handle(EditOfferCommand request, CancellationToken cancellationToken)
    {
        _logger.Info("Handling EditOfferCommandHandler");
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
        offer.EditOffer(
            request.OfferDto.Title!,
            (double)request.OfferDto.SalaryRangeMin!,
            (double)request.OfferDto.SalaryRangeMax!,
            request.OfferDto.Description!,
            (bool)request.OfferDto.IsActive!);
        await _unitOfWork.CompleteAsync();
        GetOfferVM getOfferVm = _mapper.Map<GetOfferVM>(offer);
        return new BaseMessageDto()
        {
            Object = getOfferVm,
            Message = "Offer edited successfully"
        };
    }
}