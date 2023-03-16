using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.EmployeesAuth;
using Application.EmployeesAuth.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    public class EmployeesAuthController : BasicController
    {
        public EmployeesAuthController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost("RegisterEmployee")]
        public async Task<IActionResult> Register([FromBody] RegisterEmployeeDto registerEmployeeDto)
        {
            return await _mediator.Send(new RegisterEmployeeCommand
            {
                registerEmployeeDto = registerEmployeeDto
            });
        }
    }
}

