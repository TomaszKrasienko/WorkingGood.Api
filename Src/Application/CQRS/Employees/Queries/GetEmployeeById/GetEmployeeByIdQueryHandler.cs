using System.Text;
using Application.Common.Extensions.Validation;
using Application.DTOs;
using Application.ViewModels.GetEmployee;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models.Employee;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using WorkingGood.Log;

namespace Application.CQRS.Employees.Queries;

public sealed class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, BaseMessageDto>
{
    private readonly IWgLog<GetEmployeeByIdQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<GetEmployeeByIdQuery> _validator;
    private readonly IMapper _mapper;
    public GetEmployeeByIdQueryHandler(
        IWgLog<GetEmployeeByIdQueryHandler> logger,
        IUnitOfWork unitOfWork, 
        IValidator<GetEmployeeByIdQuery> validator,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _mapper = mapper;
    }
    public async Task<BaseMessageDto> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.Info($"Getting employee by id - {request.Id}");
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.Warn(validationResult.Errors.GetErrorString());
            return new BaseMessageDto
            {
                Errors = validationResult.Errors.GetErrorsStringList()
            };
        }

        Employee employee = await _unitOfWork
            .EmployeeRepository
            .GetByIdAsync((Guid)request.Id!);
        GetEmployeeVM employeeVm = _mapper.Map<GetEmployeeVM>(employee);
        employeeVm.Initials = GetInitials(employeeVm);
        return new BaseMessageDto()
        {
            Object = employeeVm
        };
    }
    private string GetInitials(GetEmployeeVM employeeVm)
    {
        return string.Join("", employeeVm.FirstName.Substring(0, 1), employeeVm.LastName.Substring(0, 1));
    }
}