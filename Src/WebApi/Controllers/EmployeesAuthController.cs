using System.Security.Claims;
using Application.CQRS.EmployeesAuth.Commands.ChangePassword;
using Application.CQRS.EmployeesAuth.Commands.ForgotPassword;
using Application.CQRS.EmployeesAuth.Commands.Login;
using Application.CQRS.EmployeesAuth.Commands.Refresh;
using Application.CQRS.EmployeesAuth.Commands.ResetPassword;
using Application.CQRS.EmployeesAuth.Commands.VerifyEmployee;
using Application.DTOs;
using Application.DTOs.EmployeesAuth;
using Application.EmployeesAuth.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            BaseMessageDto baseMessageDto = await _mediator.Send(new RegisterEmployeeCommand
            {
                RegisterEmployeeDto = registerEmployeeDto,
                CompanyId = companyId
            });            
            if (baseMessageDto.IsSuccess())
                return Ok(baseMessageDto);
            else
                return BadRequest(baseMessageDto);
        }
        [HttpPost("VerifyEmployee/{verificationToken}")]
        public async Task<IActionResult> VerifyEmployee([FromRoute] string verificationToken)
        {
            BaseMessageDto baseMessageDto = await _mediator.Send(new VerifyEmployeeCommand
            {
                VerificationToken = verificationToken
            });            
            if (baseMessageDto.IsSuccess())
                return Ok(baseMessageDto);
            else
                return BadRequest(baseMessageDto);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] CredentialsDto credentialsDto)
        {
            BaseMessageDto baseMessageDto = await _mediator.Send(new LoginCommand
            {
                CredentialsDto = credentialsDto
            });
            if (baseMessageDto.IsSuccess())
                return Ok(baseMessageDto);
            else
                return BadRequest(baseMessageDto);
        }
        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody]RefreshDto refreshDto)
        {
            BaseMessageDto baseMessageDto = await _mediator.Send(new RefreshCommand
            {
                RefreshDto = refreshDto
            });
            if (baseMessageDto.IsSuccess())
                return Ok(baseMessageDto);
            else
                return BadRequest(baseMessageDto);
        }
        [HttpPost("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var employeeId = identity.FindFirst(EMPLOYEE_ID_KEY).Value;
            BaseMessageDto baseMessageDto = await _mediator.Send(new ChangePasswordCommand
            {
                EmployeeId = Guid.Parse(employeeId),
                ChangePasswordDto = changePasswordDto
            });
            if (baseMessageDto.IsSuccess())
                return Ok(baseMessageDto);
            else
                return BadRequest(baseMessageDto);
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            BaseMessageDto baseMessageDto = await _mediator.Send(new ForgotPasswordCommand
            {
                ForgotPasswordDto = forgotPasswordDto
            });
            if (baseMessageDto.IsSuccess())
                return Ok(baseMessageDto);
            else
                return BadRequest(baseMessageDto);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            BaseMessageDto baseMessageDto = await _mediator.Send(new ResetPasswordCommand
            {
                ResetPasswordDto = resetPasswordDto
            });
            return Ok(baseMessageDto);
        }
    }
}

