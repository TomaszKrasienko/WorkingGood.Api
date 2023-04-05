using Application.DTOs;
using Application.DTOs.EmployeesAuth;
using MediatR;

namespace Application.CQRS.EmployeesAuth.Commands.ResetPassword;

public class ResetPasswordCommand : IRequest<BaseMessageDto>
{
    public ResetPasswordDto ResetPasswordDto { get; set; } = new();
}