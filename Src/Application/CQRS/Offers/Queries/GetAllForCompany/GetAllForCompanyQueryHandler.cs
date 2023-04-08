using Application.DTOs;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using ILogger = NLog.ILogger;

namespace Application.CQRS.Offers.Queries.GetAllForCompany;

public class GetAllForCompanyQueryHandler : IRequestHandler<GetAllForCompanyQuery, BaseMessageDto>
{
    private readonly ILogger<GetAllForCompanyQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    public GetAllForCompanyQueryHandler(ILogger<GetAllForCompanyQueryHandler> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public Task<BaseMessageDto> Handle(GetAllForCompanyQuery request, CancellationToken cancellationToken)
    {
        throw new Exception();
    }
}