using System;
using Application.DTOs;
using Application.EmployeesAuth.Commands;
using Application.Extensions.Validation;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Communication;
using Domain.Models.Company;
using Domain.Models.Employee;
using FluentValidation;
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
		public RegisterEmployeeCommandHandler(ILogger<RegisterEmployeeCommandHandler> logger, IUnitOfWork unitOfWork, IValidator<RegisterEmployeeCommand> validator, IBrokerSender brokerSender)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _brokerSender = brokerSender;
        }
        public async Task<BaseMessageDto> Handle(RegisterEmployeeCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if(!validationResult.IsValid)
                return new()
                {
                    Errors = validationResult.Errors.GetErrorsStringList()
                };
            Employee employee = new Employee(
                request.RegisterEmployeeDto.FirstName!,
                request.RegisterEmployeeDto.LastName!,
                request.RegisterEmployeeDto.Email!,
                request.RegisterEmployeeDto.Password!,
                (Guid)request.CompanyId
                );
            await _unitOfWork.EmployeeRepository.AddAsync(employee);
            await _unitOfWork.CompleteAsync();
            _brokerSender.Send<RegisterMessage>(MessageDestinations.RegisterEmail, new RegisterMessage
            {
                Email = employee.Email,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                RegistrationToken = employee.VerificationToken.Token
            });
            return new ()
            {
                Message = "Added employee successfully",
                Object = employee.Id
            };
        }
    }
}

