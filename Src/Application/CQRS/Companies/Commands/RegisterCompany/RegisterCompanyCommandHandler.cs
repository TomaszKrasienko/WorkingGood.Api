using Application.Common.Extensions.Validation;
using Application.CQRS.Companies.Commands.AddCompany;
using Application.DTOs;
using Application.EmployeesAuth.Commands;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Communication;
using Domain.Models.Company;
using Domain.Models.Employee;
using FluentValidation;
using Infrastructure.Communication.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.CQRS.Companies.Commands.RegisterCompany;

public class RegisterCompanyCommandHandler : IRequestHandler<RegisterCompanyCommand,BaseMessageDto>
{
    private readonly ILogger<RegisterCompanyCommand> _logger;
    private readonly IMediator _mediator;
    public RegisterCompanyCommandHandler(
        ILogger<RegisterCompanyCommand> logger,
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    public async Task<BaseMessageDto> Handle(RegisterCompanyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling RegisterCompanyCommand");
        BaseMessageDto addCompanyMessage = await _mediator.Send(new AddCompanyCommand
        {
            CompanyDto = new()
            {
                Name = request.RegisterCompanyDto!.CompanyName
            }
        }, cancellationToken);
        if (!addCompanyMessage.IsSuccess())
            return addCompanyMessage;
        BaseMessageDto registerEmployeeMessage = await _mediator.Send(new RegisterEmployeeCommand
        {
            CompanyId = (addCompanyMessage.Object as Company).Id, 
            RegisterEmployeeDto = new() 
            {
                Email = request.RegisterCompanyDto.EmployeeEmail, 
                FirstName = request.RegisterCompanyDto.EmployeeFirstName, 
                LastName = request.RegisterCompanyDto.EmployeeLastName, 
                Password = request.RegisterCompanyDto.EmployeePassword
            }
        });
        return registerEmployeeMessage;
    }
}