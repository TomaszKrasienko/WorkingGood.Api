using Application.DTOs;
using Application.ViewModels.Offer;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using IMapper = AutoMapper.IMapper;

namespace Application.CQRS.Offers.Queries.GetActiveOffers;

public class GetActiveOffersQueryHandler : IRequestHandler<GetActiveOffersQuery, BaseMessageDto>
{
    private readonly ILogger<GetActiveOffersQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public GetActiveOffersQueryHandler(ILogger<GetActiveOffersQueryHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<BaseMessageDto> Handle(GetActiveOffersQuery request, CancellationToken cancellationToken)
    {
        var offersList = await _unitOfWork
            .OffersRepository
            .GetAllActive();
        List<GetOfferVM> getOfferVmList = new ();
        foreach (var offer in offersList)
        {
            getOfferVmList.Add(_mapper.Map<GetOfferVM>(offer));
        }
        return new BaseMessageDto
        {
            Object = getOfferVmList
        };
    }
}