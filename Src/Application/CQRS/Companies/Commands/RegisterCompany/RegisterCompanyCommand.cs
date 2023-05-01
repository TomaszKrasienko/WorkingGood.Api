using Application.DTOs;
using Application.DTOs.Companies;
using MediatR;

namespace Application.CQRS.Companies.Commands.RegisterCompany;

public class RegisterCompanyCommand : IRequest<BaseMessageDto>
{
    public RegisterCompanyDto? RegisterCompanyDto { get; set; }
}