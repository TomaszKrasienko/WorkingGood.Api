using System;
using Application.Common.Extensions.Validation;
using Application.DTOs;
using Application.EmployeesAuth.Commands;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Communication;
using Domain.Models.Company;
using Domain.Models.Employee;
using FluentValidation;
using Infrastructure.Common.ConfigModels;
using Infrastructure.Communication.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.EmployeesAuth.Commands.RegisterEmployee
{
	public class RegisterEmployeeCommandHandler : IRequestHandler<RegisterEmployeeCommand, BaseMessageDto>
    {
        private readonly ILogger<RegisterEmployeeCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<RegisterEmployeeCommand> _validator;
        private readonly IBrokerSender _brokerSender;
        private readonly AddressesConfig _addressesConfig;
		public RegisterEmployeeCommandHandler(
            ILogger<RegisterEmployeeCommandHandler> logger,
            IUnitOfWork unitOfWork,
            IValidator<RegisterEmployeeCommand> validator,
            IBrokerSender brokerSender,
            AddressesConfig addressesConfig)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _brokerSender = brokerSender;
            _addressesConfig = addressesConfig;
        }
        public async Task<BaseMessageDto> Handle(RegisterEmployeeCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling RegisterEmployeeCommand");
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning(validationResult.Errors.GetErrorString());
                return new()
                {
                    Errors = validationResult.Errors.GetErrorsStringList()
                };
            }
            Employee employee = new Employee(
                request.RegisterEmployeeDto.FirstName!,
                request.RegisterEmployeeDto.LastName!,
                request.RegisterEmployeeDto.Email!,
                request.RegisterEmployeeDto.Password!,
                (Guid)request.CompanyId!
                );
            await _unitOfWork.EmployeeRepository.AddAsync(employee);
            await _unitOfWork.CompleteAsync();
            _brokerSender.Send<RegisterMessage>(MessageDestinations.RegisterEmail, new RegisterMessage
            {
                Email = employee.Email.EmailAddress,
                FirstName = employee.EmployeeName.FirstName,
                LastName = employee.EmployeeName.LastName,
                RegistrationUrl= $"{_addressesConfig.RegistrationUrl}/{employee.VerificationToken.Token}"
            });
            return new ()
            {
                Message = "Added employee successfully",
                Object = employee.Id
            };
        }
    }
}

