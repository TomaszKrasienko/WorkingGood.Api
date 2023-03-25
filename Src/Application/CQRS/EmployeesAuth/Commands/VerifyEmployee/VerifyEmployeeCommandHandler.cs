using Application.DTOs;
using Application.Extensions.Validation;
using Domain.Interfaces;
using Domain.Models.Company;
using Domain.Models.Employee;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.EmployeesAuth.Commands.VerifyEmployee;

public class VerifyEmployeeCommandHandler : IRequestHandler<VerifyEmployeeCommand, IActionResult>
{
    private readonly ILogger<VerifyEmployeeCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<VerifyEmployeeCommand> _validator;
    public VerifyEmployeeCommandHandler(ILogger<VerifyEmployeeCommandHandler> logger, IUnitOfWork unitOfWork, IValidator<VerifyEmployeeCommand> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<IActionResult> Handle(VerifyEmployeeCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return new BadRequestObjectResult(new BaseMessageDto
            {
                Errors = validationResult.Errors.GetErrorsStringList()
            });
        Employee employee = await _unitOfWork
            .EmployeeRepository
            .GetByVerificationToken(request.VerificationToken!);
        employee.Activate();
        await _unitOfWork.CompleteAsync();
        return new OkObjectResult(new BaseMessageDto
        {
            Message = "Account activated successfully"
        });
    }
}