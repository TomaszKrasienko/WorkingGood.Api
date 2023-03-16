using System;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.EmployeesAuth.Commands
{
	public class RegisterEmployeeCommandHandler : IRequestHandler<RegisterEmployeeCommand, IActionResult>
	{
		public RegisterEmployeeCommandHandler()
		{
		}
        public Task<IActionResult> Handle(RegisterEmployeeCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

