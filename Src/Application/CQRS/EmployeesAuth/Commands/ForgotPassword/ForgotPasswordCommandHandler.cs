using Application.Common.Extensions.Validation;
using Application.DTOs;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Communication;
using Domain.Models.Employee;
using FluentValidation;
using Infrastructure.Common.ConfigModels;
using Infrastructure.Communication.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WorkingGood.Log;

namespace Application.CQRS.EmployeesAuth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, BaseMessageDto>
{
    private readonly IWgLog<ForgotPasswordCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<ForgotPasswordCommand> _validator;
    private readonly IBrokerSender _brokerSender;
    private readonly AddressesConfig _addressesConfig;
    public ForgotPasswordCommandHandler(
        IWgLog<ForgotPasswordCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IValidator<ForgotPasswordCommand> validator,
        IBrokerSender brokerSender,
        AddressesConfig addressesConfig)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _brokerSender = brokerSender;
        _addressesConfig = addressesConfig;
    }
    public async Task<BaseMessageDto> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {            
        _logger.Info("Handling ForgotPasswordCommand");
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.Info(validationResult.Errors.GetErrorString());
            return new()
            {
                Errors = validationResult.Errors.GetErrorsStringList()
            };
        }
        Employee employee = await _unitOfWork.EmployeeRepository.GetByEmailAsync(request.ForgotPasswordDto.EmployeeEmail!);
        employee.SetResetToken();
        await _unitOfWork.CompleteAsync();
        _brokerSender.Send<ForgotPasswordMessage>(MessageDestinations.ForgotPasswordEmail, new ForgotPasswordMessage
        {
            Email = employee.Email.EmailAddress,
            FirstName = employee.EmployeeName.FirstName,
            LastName = employee.EmployeeName.LastName,
            ForgotPasswordUrl = $"{_addressesConfig.ForgotPasswordUrl}/{employee!.ResetToken!.Token!}"
        });
        return new ()
        {
            Message = "Message sent to employee email"
        };
    }
}