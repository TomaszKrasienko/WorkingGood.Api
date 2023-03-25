using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.CQRS.EmployeesAuth.Commands.VerifyEmployee;

public record VerifyEmployeeCommand : IRequest<BaseMessageDto>
{
    public string? VerificationToken { get; init; }
}