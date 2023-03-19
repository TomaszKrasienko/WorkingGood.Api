using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        protected readonly IMediator _mediator;
        protected const string EMPLOYEE_ID_KEY = "EmployeeId";
        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}

