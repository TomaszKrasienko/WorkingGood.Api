using Application.DTOs;
using Application.DTOs.EmployeesAuth;
using Application.Extensions.Validation;
using Domain.Interfaces;
using Domain.Models.Employee;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.EmployeesAuth.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, IActionResult>
{
    private readonly ILogger<ChangePasswordCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<ChangePasswordCommand> _validator;
    public ChangePasswordCommandHandler(ILogger<ChangePasswordCommandHandler> logger, IUnitOfWork unitOfWork, IValidator<ChangePasswordCommand> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<IActionResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return new BadRequestObjectResult(new BaseMessageDto
            {
                Errors = validationResult.Errors.GetErrorsStringList()
            });
        Employee employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(request.EmployeeId);
        if (!(employee.IsPasswordMatch(request.ChangePasswordDto.OldPassword!)))
            return new BadRequestObjectResult(new BaseMessageDto
            {
                Errors = "Password is incorrect"
            });
        employee.SetNewPassword(request.ChangePasswordDto.NewPassword!);
        await _unitOfWork.CompleteAsync();
        return new OkObjectResult(new BaseMessageDto
        {
            Message = "Password changed"
        });
    }
}