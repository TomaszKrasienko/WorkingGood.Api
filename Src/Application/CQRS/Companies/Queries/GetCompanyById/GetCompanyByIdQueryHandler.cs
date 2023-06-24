using Application.Common.Extensions.Validation;
using Application.DTOs;
using Application.ViewModels.Company;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models.Company;
using Domain.Services;
using FluentValidation;
using MediatR;
using WorkingGood.Log;

namespace Application.CQRS.Companies.Queries.GetCompanyById;

public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, BaseMessageDto>
{
    private readonly IWgLog<GetCompanyByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<GetCompanyByIdQuery> _validator;
    
    public GetCompanyByIdQueryHandler(
        IWgLog<GetCompanyByIdQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<GetCompanyByIdQuery> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _validator = validator;
    }
    public async Task<BaseMessageDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.Info($"Handling {request.CompanyId}");
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.Warn(validationResult.Errors.GetErrorString());
            return new BaseMessageDto()
            {
                Errors = validationResult.Errors.GetErrorsStringList()
            };
        }

        Company company = await _unitOfWork
            .CompanyRepository
            .GetByIdAsync((Guid) request.CompanyId!);
        CompanyVM companyVm = _mapper.Map<CompanyVM>(company);
        return new BaseMessageDto()
        {
            Object = companyVm
        };
    }
}