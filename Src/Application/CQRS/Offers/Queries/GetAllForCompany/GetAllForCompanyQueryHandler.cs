using Application.Common.Extensions.Validation;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Models.Employee;
using Domain.Models.Offer;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using ILogger = NLog.ILogger;

namespace Application.CQRS.Offers.Queries.GetAllForCompany;

public class GetAllForCompanyQueryHandler : IRequestHandler<GetAllForCompanyQuery, BaseMessageDto>
{
    private readonly ILogger<GetAllForCompanyQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<GetAllForCompanyQuery> _validator;
    public GetAllForCompanyQueryHandler(ILogger<GetAllForCompanyQueryHandler> logger, IUnitOfWork unitOfWork, IValidator<GetAllForCompanyQuery> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<BaseMessageDto> Handle(GetAllForCompanyQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetAllForCompanyQuery");
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(validationResult.Errors.GetErrorString());
            return new BaseMessageDto
            {
                Errors = validationResult.Errors.GetErrorString()
            };
        }
        Employee employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId);
        List<Employee> employeesList = await _unitOfWork.EmployeeRepository.GetByCompanyIdAsync(employee.CompanyId);
        List<Offer> offers = await _unitOfWork
            .OffersRepository
            .GetAllForEmployees(employeesList
                .Select(x => x.Id)
                .ToList());
        return new()
        {
            Object = offers
        };
    }
}