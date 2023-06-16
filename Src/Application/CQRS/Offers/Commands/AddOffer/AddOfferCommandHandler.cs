using Application.Common.Extensions.Validation;
using Application.DTOs;
using Application.DTOs.Offers;
using Application.ViewModels.Offer;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models.Offer;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using WorkingGood.Log;

namespace Application.CQRS.Offers.Commands.AddOffer;

public class AddOfferCommandHandler : IRequestHandler<AddOfferCommand, BaseMessageDto>
{
    private readonly IWgLog<AddOfferCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<AddOfferCommand> _validator;
    private readonly IMapper _mapper;
    public AddOfferCommandHandler(
        IWgLog<AddOfferCommandHandler> logger, 
        IUnitOfWork unitOfWork, 
        IValidator<AddOfferCommand> validator,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _mapper = mapper;
    }
    public async Task<BaseMessageDto> Handle(AddOfferCommand request, CancellationToken cancellationToken)
    {            
        _logger.Info("Handling AddOfferCommand");
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.Warn(validationResult.Errors.GetErrorString());
            return new BaseMessageDto
            {
                Errors = validationResult.Errors.GetErrorsStringList()
            };
        }

        Position? position = await _unitOfWork
            .OffersRepository
            .GetPositionByNameAsync(request.OfferDto.PositionType!);
        Offer offer;
        if (position is null)
        {
            offer = new Offer(
                request.OfferDto.Title!,
                request.OfferDto.PositionType!,
                (double) request.OfferDto.SalaryRangeMin!,
                (double) request.OfferDto.SalaryRangeMax!,
                request.OfferDto.Description!,
                (Guid) request.EmployeeId!,
                (bool) request.OfferDto.IsActive!
            );
        }
        else
        {            
            offer = new Offer(
                request.OfferDto.Title!,
                position,
                (double) request.OfferDto.SalaryRangeMin!,
                (double) request.OfferDto.SalaryRangeMax!,
                request.OfferDto.Description!,
                (Guid) request.EmployeeId!,
                (bool) request.OfferDto.IsActive!
            );
        }
        await _unitOfWork.OffersRepository.AddAsync(offer);
        await _unitOfWork.CompleteAsync();
        GetOfferVM getOfferVm = _mapper.Map<GetOfferVM>(offer);
        return new()
        {
            Message = "Offer added successfully",
            Object = getOfferVm
        };
    }
}