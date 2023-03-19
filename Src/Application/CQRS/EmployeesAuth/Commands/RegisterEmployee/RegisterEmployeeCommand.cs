using System;
using Application.DTOs.EmployeesAuth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.EmployeesAuth.Commands
{
	public record RegisterEmployeeCommand : IRequest<IActionResult>
	{
		public Guid? CompanyId { get; init; }
		public RegisterEmployeeDto RegisterEmployeeDto { get; init; } = new();
	}
}

