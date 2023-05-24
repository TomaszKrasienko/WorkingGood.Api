using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Exceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IMediator Mediator;
        private const string EMPLOYEE_ID_KEY = "EmployeeId";
        public BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }
        protected Guid? GetEmployeeId()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string? employeeId = HttpContext.User.FindFirst(EMPLOYEE_ID_KEY)?.Value;
            return employeeId is null ? null : Guid.Parse(employeeId);
        }
    }
}

