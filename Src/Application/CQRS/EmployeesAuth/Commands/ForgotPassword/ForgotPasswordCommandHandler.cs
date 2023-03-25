using Application.DTOs;
using Application.Extensions.Validation;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Communication;
using Domain.Models.Employee;
using FluentValidation;
using Infrastructure.Communication.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.EmployeesAuth.Commands.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, IActionResult>
{
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<ForgotPasswordCommand> _validator;
    private readonly IBrokerSender _brokerSender;
    public ForgotPasswordCommandHandler(ILogger<ForgotPasswordCommandHandler> logger, IUnitOfWork unitOfWork, IValidator<ForgotPasswordCommand> validator, IBrokerSender brokerSender)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _validator = validator;
        _brokerSender = brokerSender;
    }
    public async Task<IActionResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return new BadRequestObjectResult(new BaseMessageDto
            {
                Errors = validationResult.Errors.GetErrorsStringList()
            });
        Employee employee = await _unitOfWork.EmployeeRepository.GetByEmail(request.ForgotPasswordDto.EmployeeEmail!);
        employee.SetResetToken();
        await _unitOfWork.CompleteAsync();
        _brokerSender.Send<ForgotPasswordMessage>(MessageDestinations.ForgotPasswordEmail, new ForgotPasswordMessage
        {
            Email = employee.Email,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            ForgotPasswordToken = employee!.ResetToken!.Token!
        });
        return new OkObjectResult(new BaseMessageDto
        {
            Message = "Message sent to employee email"
        });
    }
}