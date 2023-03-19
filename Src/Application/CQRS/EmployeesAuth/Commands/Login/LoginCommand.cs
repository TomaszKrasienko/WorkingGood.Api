using Application.DTOs.EmployeesAuth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.CQRS.EmployeesAuth.Commands.Login;

public record LoginCommand : IRequest<IActionResult>
{
    public CredentialsDto CredentialsDto { get; init; } = new();
}