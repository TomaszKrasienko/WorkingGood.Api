using Application.Common.Extensions.Validation;
using Application.DTOs;
using Application.ViewModels.Offer;
using AutoMapper;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Offers.Queries.GetOfferById;

public class GetOfferByIdCommandHandler : IRequestHandler<GetOfferByIdCommand, BaseMessageDto>
{
    private readonly ILogger<GetOfferByIdCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<GetOfferByIdCommand> _validator;
    private readonly IMapper _mapper;
    public GetOfferByIdCommandHandler(
        ILogger<GetOfferByIdCommandHandler> logger, 
        IUnitOfWork unitOfWork, 
        IValidator<GetOfferByIdCommand> validator,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _mapper = mapper;
    }
    public async Task<BaseMessageDto> Handle(GetOfferByIdCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(validationResult.Errors.GetErrorString());
            return new()
            {
                Errors = validationResult.Errors.GetErrorsStringList()
            };
        }
        var offer = await _unitOfWork
            .OffersRepository
            .GetByIdAsync(request.OfferId);
        GetOfferVM getOfferVm = _mapper.Map<GetOfferVM>(offer);
        return new()
        {
            Object = getOfferVm
        };
    }
}