using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.CQRS.EmployeesAuth.Commands.ChangePassword;
using Application.CQRS.EmployeesAuth.Commands.ForgotPassword;
using Application.CQRS.EmployeesAuth.Commands.Login;
using Application.CQRS.EmployeesAuth.Commands.Refresh;
using Application.CQRS.EmployeesAuth.Commands.VerifyEmployee;
using Application.DTOs.EmployeesAuth;
using Application.EmployeesAuth.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    public class EmployeesAuthController : BaseController
    {
        public EmployeesAuthController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost("RegisterEmployee/{companyId}")]
        public async Task<IActionResult> Register([FromBody] RegisterEmployeeDto registerEmployeeDto, [FromRoute]Guid companyId)
        {
            return await _mediator.Send(new RegisterEmployeeCommand
            {
                RegisterEmployeeDto = registerEmployeeDto,
                CompanyId = companyId
            });
        }
        [HttpPost("VerifyEmployee/{verificationToken}")]
        public async Task<IActionResult> VerifyEmployee([FromRoute] string verificationToken)
        {
            return await _mediator.Send(new VerifyEmployeeCommand
            {
                VerificationToken = verificationToken
            });
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] CredentialsDto credentialsDto)
        {
            return await _mediator.Send(new LoginCommand
            {
                CredentialsDto = credentialsDto
            });
        }
        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody]RefreshDto refreshDto)
        {
            return await _mediator.Send(new RefreshCommand
            {
                RefreshDto = refreshDto
            });
        }
        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var employeeId = identity.FindFirst(EMPLOYEE_ID_KEY).Value;
            return await _mediator.Send(new ChangePasswordCommand
            {
                EmployeeId = Guid.Parse(employeeId),
                ChangePasswordDto = changePasswordDto
            });
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            return await _mediator.Send(new ForgotPasswordCommand
            {
                ForgotPasswordDto = forgotPasswordDto
            });
        }
    }
}

