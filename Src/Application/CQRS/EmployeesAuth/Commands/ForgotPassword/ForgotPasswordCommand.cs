using Application.DTOs;
using Application.DTOs.EmployeesAuth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.CQRS.EmployeesAuth.Commands.ForgotPassword;

public record ForgotPasswordCommand : IRequest<BaseMessageDto>
{
    public ForgotPasswordDto ForgotPasswordDto { get; init; } = new();
}