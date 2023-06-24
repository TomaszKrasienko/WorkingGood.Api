using Application.DTOs;
using Application.ViewModels.Company;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models.Company;
using Domain.Models.Employee;
using Domain.Models.Offer;
using MediatR;
using WorkingGood.Log;

namespace Application.CQRS.Companies.Queries.GetCompanyByOfferId;

public class GetCompanyByOfferIdQueryHandler : IRequestHandler<GetCompanyByOfferIdQuery, BaseMessageDto>
{
    private readonly IWgLog<GetCompanyByOfferIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper; 
    
    public GetCompanyByOfferIdQueryHandler(
        IWgLog<GetCompanyByOfferIdQueryHandler> logger,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<BaseMessageDto> Handle(GetCompanyByOfferIdQuery request, CancellationToken cancellationToken)
    {
         Offer offer = await _unitOfWork.OffersRepository.GetByIdAsync(request.OfferId);
         Employee employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(offer.AuthorId);
         Company company = await _unitOfWork.CompanyRepository.GetByIdAsync(employee.CompanyId);
         CompanyVM companyVm = _mapper.Map<CompanyVM>(company);
         return new()
         {
             Object = companyVm
         };
    }
}