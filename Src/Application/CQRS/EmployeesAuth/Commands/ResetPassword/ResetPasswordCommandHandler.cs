using Application.Common.Extensions.Validation;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Models.Employee;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.EmployeesAuth.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, BaseMessageDto>
{
    private readonly ILogger<ResetPasswordCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<ResetPasswordCommand> _validator;
    public ResetPasswordCommandHandler(ILogger<ResetPasswordCommandHandler> logger, IUnitOfWork unitOfWork, IValidator<ResetPasswordCommand> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<BaseMessageDto> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {            
        _logger.LogInformation("Handling ResetPasswordCommand");
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning(validationResult.Errors.GetErrorString());
            return new()
            {
                Errors = validationResult.Errors.GetErrorsStringList()
            };
        }
        Employee employee = await _unitOfWork.EmployeeRepository.GetByResetToken(request.ResetPasswordDto.ResetToken!);
        employee.SetNewPassword(request.ResetPasswordDto.NewPassword!);
        await _unitOfWork.CompleteAsync();
        return new()
        {
            Message = "The password has been changed"
        };
    }
}