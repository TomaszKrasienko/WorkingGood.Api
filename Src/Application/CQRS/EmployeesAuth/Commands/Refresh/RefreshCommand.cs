using System.Security.Principal;
using Application.DTOs;
using Application.DTOs.EmployeesAuth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.CQRS.EmployeesAuth.Commands.Refresh;
public record RefreshCommand : IRequest<RefreshResponseDto>
{
    public RefreshDto RefreshDto { get; init; } = new();
}