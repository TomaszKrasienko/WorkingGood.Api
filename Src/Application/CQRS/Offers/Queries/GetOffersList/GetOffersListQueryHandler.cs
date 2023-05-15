using Application.Common.Extensions.Validation;
using Application.DTOs;
using Application.ViewModels.Base;
using Application.ViewModels.Offer;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models.Employee;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Offers.Queries.GetOffersList;

public class GetOffersListQueryHandler : IRequestHandler<GetOffersListQuery, BaseMessageDto>
{
    private readonly ILogger<GetOffersListQueryHandler> _logger;
    private readonly IValidator<GetOffersListQuery> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public GetOffersListQueryHandler(
        ILogger<GetOffersListQueryHandler> logger, 
        IValidator<GetOffersListQuery> validator,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<BaseMessageDto> Handle(GetOffersListQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return new BaseMessageDto()
            {
                Errors = validationResult.Errors.GetErrorsStringList(),
            };
        List<Guid> employeeIdList = new();
        if (request.EmployeeId is not null && request.GetOffersListRequestDto.CompanyOffers == true)
        {
            Employee employee = await _unitOfWork.EmployeeRepository
                .GetByIdAsync((Guid)request.EmployeeId);
            employeeIdList = (await _unitOfWork
                .EmployeeRepository
                .GetByCompanyIdAsync(employee.CompanyId))
                    .Select(x => x.Id)
                    .ToList();
        }
        var offers = await _unitOfWork
            .OffersRepository
            .GetAllAsync(
                request.GetOffersListRequestDto.PageNumber,
                request.GetOffersListRequestDto.PageSize,
                employeeIdList,
                request.GetOffersListRequestDto.IsActive);
        List<GetOfferVM> getOfferVms = _mapper.Map<List<GetOfferVM>>(offers);
        return new BaseMessageDto()
        {
            Object = getOfferVms,
            MetaData = await GetMetaData(
                employeeIdList, 
                request.GetOffersListRequestDto.IsActive,
                request.GetOffersListRequestDto.PageNumber,
                request.GetOffersListRequestDto.PageSize) 
        };
    }
    private async Task<MetaDataVm> GetMetaData(List<Guid> employeesIds, bool? isActive, int currentPage, int pageSize)
    {
        var totalRecords = await _unitOfWork.OffersRepository.CountAll(employeesIds, isActive);
        var totalPages = (double) totalRecords / (double) pageSize;
        var roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
        return new MetaDataVm()
        {
            CurrentPage = currentPage,
            HasNext = roundedTotalPages != currentPage,
            HasPrevious = currentPage != 1,
            PageSize = pageSize,
            TotalCount = totalRecords,
            TotalPages = roundedTotalPages
        };
    }
}