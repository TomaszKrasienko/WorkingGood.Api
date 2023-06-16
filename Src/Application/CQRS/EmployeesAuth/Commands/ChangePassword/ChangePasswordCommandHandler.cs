using Application.Common.Extensions.Validation;
using Application.DTOs;
using Domain.Interfaces;
using Domain.Models.Employee;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using WorkingGood.Log;

namespace Application.CQRS.EmployeesAuth.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, BaseMessageDto>
{
    private readonly IWgLog<ChangePasswordCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<ChangePasswordCommand> _validator;
    public ChangePasswordCommandHandler(IWgLog<ChangePasswordCommandHandler> logger, IUnitOfWork unitOfWork, IValidator<ChangePasswordCommand> validator)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }
    public async Task<BaseMessageDto> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        _logger.Info("Handling ChangePasswordCommand");
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.Info(validationResult.Errors.GetErrorString());
            return new()
            {
                Errors = validationResult.Errors.GetErrorsStringList()
            };
        }
        Employee employee = await _unitOfWork.EmployeeRepository.GetByIdAsync((Guid)request.EmployeeId!);
        if (!(employee.IsPasswordMatch(request.ChangePasswordDto.OldPassword!)))
        {
            _logger.Info("Password is incorrect");
            return new()
            {
                Errors = "Password is incorrect"
            };
        }
        employee.SetNewPassword(request.ChangePasswordDto.NewPassword!);
        await _unitOfWork.CompleteAsync();
        return new ()
        {
            Message = "Password changed"
        };
    }
}