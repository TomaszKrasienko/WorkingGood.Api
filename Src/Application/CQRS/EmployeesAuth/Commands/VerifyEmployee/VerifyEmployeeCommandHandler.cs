using Application.Common.Extensions.Validation;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Models.Company;
using Domain.Models.Employee;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorkingGood.Log;

namespace Application.CQRS.EmployeesAuth.Commands.VerifyEmployee;

public class VerifyEmployeeCommandHandler : IRequestHandler<VerifyEmployeeCommand, BaseMessageDto>
{
    private readonly IWgLog<VerifyEmployeeCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<VerifyEmployeeCommand> _validator;
    public VerifyEmployeeCommandHandler(IWgLog<VerifyEmployeeCommandHandler> logger, IUnitOfWork unitOfWork, IValidator<VerifyEmployeeCommand> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<BaseMessageDto> Handle(VerifyEmployeeCommand request, CancellationToken cancellationToken)
    {
        _logger.Info("Handling VerifyEmployeeCommand");
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.Info(validationResult.Errors.GetErrorString());
            return new()
            {
                Errors = validationResult.Errors.GetErrorsStringList()
            };
        }
        Employee employee = await _unitOfWork
            .EmployeeRepository
            .GetByVerificationTokenAsync(request.VerificationToken!);
        employee.Activate();
        await _unitOfWork.CompleteAsync();
        return new()
        {
            Message = "Account activated successfully"
        };
    }
}