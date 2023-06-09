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
using WorkingGood.Log;

namespace Application.CQRS.Offers.Queries.GetOffersList;

public class GetOffersListQueryHandler : IRequestHandler<GetOffersListQuery, BaseMessageDto>
{
    private readonly IWgLog<GetOffersListQueryHandler> _logger;
    private readonly IValidator<GetOffersListQuery> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public GetOffersListQueryHandler(
        IWgLog<GetOffersListQueryHandler> logger, 
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
        _logger.Info("Handling GetOffersListQueryHandler");
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
                pageNumber: request.GetOffersListRequestDto.PageNumber,
                pageSize: request.GetOffersListRequestDto.PageSize,
                employeeIdList: employeeIdList,
                isActive: request.GetOffersListRequestDto.IsActive,
                employeeId: request.EmployeeId,
                rateFrom: request.GetOffersListRequestDto.RateFrom,
                rateTo: request.GetOffersListRequestDto.RateTo,
                searchPhrase: request.GetOffersListRequestDto.SearchPhrase);
        List<GetOfferVM> getOfferVms = _mapper.Map<List<GetOfferVM>>(offers);
        return new BaseMessageDto()
        {
            Object = getOfferVms,
            MetaData = await GetMetaData(
                employeeIdList, 
                request.GetOffersListRequestDto.IsActive,
                request.GetOffersListRequestDto.PageNumber,
                request.GetOffersListRequestDto.PageSize,
                employeeId: request.EmployeeId,
                rateFrom: request.GetOffersListRequestDto.RateFrom,
                rateTo: request.GetOffersListRequestDto.RateTo,
                searchPhrase: request.GetOffersListRequestDto.SearchPhrase) 
        };
    }
    private async Task<MetaDataVm> GetMetaData(
        List<Guid> employeesIds, 
        bool? isActive, 
        int currentPage, 
        int pageSize,
        Guid? employeeId,
        int? rateFrom,
        int? rateTo,
        string? searchPhrase)
    {
        var totalRecords = await _unitOfWork.OffersRepository.CountAll(
            employeesIds, 
            isActive,
            employeeId,
            rateFrom,
            rateTo,
            searchPhrase);
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