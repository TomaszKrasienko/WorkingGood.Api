using System.Security.Claims;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IMediator Mediator;
        public BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }
        protected Guid? GetEmployeeId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string? employeeId = HttpContext.User.FindFirst(TokenKey.EmployeeId.ToString())?.Value;
            return employeeId is null ? null : Guid.Parse(employeeId);
        }
        
        protected Guid? GetCompanyId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string? companyId = HttpContext.User.FindFirst(TokenKey.CompanyId.ToString())?.Value;
            return companyId is null ? null : Guid.Parse(companyId);
        }
    }
}

