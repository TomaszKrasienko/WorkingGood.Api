using Application.Common.Extensions.Validation;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Models.Employee;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using WorkingGood.Log;

namespace Application.CQRS.EmployeesAuth.Commands.ResetPassword;

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, BaseMessageDto>
{
    private readonly IWgLog<ResetPasswordCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<ResetPasswordCommand> _validator;
    public ResetPasswordCommandHandler(IWgLog<ResetPasswordCommandHandler> logger, IUnitOfWork unitOfWork, IValidator<ResetPasswordCommand> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<BaseMessageDto> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {            
        _logger.Info("Handling ResetPasswordCommand");
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.Info(validationResult.Errors.GetErrorString());
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