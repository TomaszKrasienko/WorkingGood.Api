using System.Security.Claims;
using Application.CQRS.Employees.Queries;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Exceptions;

namespace WebApi.Controllers;

[Authorize]
[Route("employees")]
public class EmployeeController : BaseController
{
    public EmployeeController(IMediator mediator) : base(mediator) { }
    [HttpGet("getLoggedEmployee")]
    public async Task<IActionResult> GetLoggedEmployee()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        string employeeId = identity!.FindFirst(EMPLOYEE_ID_KEY)!.Value ?? throw new UserNotFoundException();
        BaseMessageDto baseMessageDto = await Mediator.Send(new GetEmployeeByIdQuery
        {
            Id = Guid.Parse(employeeId)
        });
        if (baseMessageDto.IsSuccess())
            return Ok(baseMessageDto);
        else
            return BadRequest(baseMessageDto);
    }
}