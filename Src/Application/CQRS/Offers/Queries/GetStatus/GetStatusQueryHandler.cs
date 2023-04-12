using Application.DTOs;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Offers.Queries.GetOfferStatus;

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
    public Task<BaseMessageDto> Handle(GetStatusQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}