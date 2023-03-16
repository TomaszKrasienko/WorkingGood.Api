using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class BasicController : Controller
    {
        protected readonly IMediator _mediator;
        public BasicController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}

