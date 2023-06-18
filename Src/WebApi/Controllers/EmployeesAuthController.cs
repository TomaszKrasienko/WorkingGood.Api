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
using WebApi.Common.Exceptions;

namespace WebApi.Controllers
{
    [Route("employeesAuth")]
    public class EmployeesAuthController : BaseController
    {
        public EmployeesAuthController(IMediator mediator) : base(mediator) { }
        [HttpPost("registerEmployee/{companyId}")]
        public async Task<IActionResult> Register([FromBody] RegisterEmployeeDto registerEmployeeDto, [FromRoute]Guid companyId)
        {
            //Todo: Dodać potwierdzenie hasła i sprawdzenie tego 
            BaseMessageDto baseMessageDto = await Mediator.Send(new RegisterEmployeeCommand
            {
                RegisterEmployeeDto = registerEmployeeDto,
                CompanyId = companyId
            });            
            if (baseMessageDto.IsSuccess())
                return Ok(baseMessageDto);
            else
                return BadRequest(baseMessageDto);
        }
        [HttpPost("verifyEmployee/{verificationToken}")]
        public async Task<IActionResult> VerifyEmployee([FromRoute] string verificationToken)
        {
            BaseMessageDto baseMessageDto = await Mediator.Send(new VerifyEmployeeCommand
            {
                VerificationToken = verificationToken
            });            
            if (baseMessageDto.IsSuccess())
                return Ok(baseMessageDto);
            else
                return BadRequest(baseMessageDto);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredentialsDto credentialsDto)
        {
            BaseMessageDto baseMessageDto = await Mediator.Send(new LoginCommand
            {
                CredentialsDto = credentialsDto
            });
            if (baseMessageDto.IsSuccess())
                return Ok(baseMessageDto);
            else
                return BadRequest(baseMessageDto);
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody]RefreshDto refreshDto)
        {
            RefreshResponseDto refreshResponseDto = await Mediator.Send(new RefreshCommand
            {
                RefreshDto = refreshDto
            });
            if (refreshResponseDto.IsSuccess())
                return Ok(refreshResponseDto as BaseMessageDto);
            else if (!refreshResponseDto.IsSuccess() && refreshResponseDto.IsAuthorized)
                return Unauthorized(refreshResponseDto as BaseMessageDto);
            else
                return BadRequest(refreshResponseDto as BaseMessageDto);
        }
        [HttpPost("changePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            BaseMessageDto baseMessageDto = await Mediator.Send(new ChangePasswordCommand
            {
                EmployeeId = GetEmployeeId(),
                ChangePasswordDto = changePasswordDto
            });
            if (baseMessageDto.IsSuccess())
                return Ok(baseMessageDto);
            else
                return BadRequest(baseMessageDto);
        }
        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            BaseMessageDto baseMessageDto = await Mediator.Send(new ForgotPasswordCommand
            {
                ForgotPasswordDto = forgotPasswordDto
            });
            if (baseMessageDto.IsSuccess())
                return Ok(baseMessageDto);
            else
                return BadRequest(baseMessageDto);
        }
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            //Todo: Usuwanie refresh Tokena
            BaseMessageDto baseMessageDto = await Mediator.Send(new ResetPasswordCommand
            {
                ResetPasswordDto = resetPasswordDto
            });
            return Ok(baseMessageDto);
        }
    }
}

