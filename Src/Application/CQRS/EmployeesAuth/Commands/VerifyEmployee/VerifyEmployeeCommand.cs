using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.CQRS.EmployeesAuth.Commands.VerifyEmployee;

public record VerifyEmployeeCommand : IRequest<IActionResult>
{
    public string? VerificationToken { get; init; }
}