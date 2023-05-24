using Application.DTOs;
using Application.DTOs.EmployeesAuth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.CQRS.EmployeesAuth.Commands.ChangePassword;

public record ChangePasswordCommand : IRequest<BaseMessageDto>
{
    public Guid? EmployeeId { get; set; }
    public ChangePasswordDto ChangePasswordDto { get; init; } = new();
}