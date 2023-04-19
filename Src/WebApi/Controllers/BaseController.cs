using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IMediator Mediator;
        protected const string EMPLOYEE_ID_KEY = "EmployeeId";
        public BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }
    }
}

