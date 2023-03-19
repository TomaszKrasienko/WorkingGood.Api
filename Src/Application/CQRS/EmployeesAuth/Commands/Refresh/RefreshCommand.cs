using System.Security.Principal;
using Application.DTOs.EmployeesAuth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.CQRS.EmployeesAuth.Commands.Refresh;
public record RefreshCommand : IRequest<IActionResult>
{
    public RefreshDto RefreshDto { get; init; }   
}