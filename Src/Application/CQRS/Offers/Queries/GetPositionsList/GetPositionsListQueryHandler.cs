using Application.DTOs;
using Application.DTOs.Offers;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models.Offer;
using MediatR;
using Microsoft.Extensions.Logging;
using NLog;
using WorkingGood.Log;

namespace Application.CQRS.Offers.Queries.GetPositions;

public class GetPositionsListQueryHandler : IRequestHandler<GetPositionsListQuery, BaseMessageDto>
{
    private readonly IWgLog<GetPositionsListQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    
    public GetPositionsListQueryHandler(IWgLog<GetPositionsListQueryHandler> logger, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<BaseMessageDto> Handle(GetPositionsListQuery request, CancellationToken cancellationToken)
    {
        _logger.Info("Handling GetPositionsListQuery");
        List<Position> positionsList = await _unitOfWork.OffersRepository.GetPositionsAsync();
        List<GetPositionDto> getPositionDtoList = _mapper.Map<List<GetPositionDto>>(positionsList);
        return new BaseMessageDto()
        {
            Object = getPositionDtoList
        };
    }
}