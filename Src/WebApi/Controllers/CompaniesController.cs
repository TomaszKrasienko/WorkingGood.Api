using Application.CQRS.Companies.Commands;
using Application.DTOs.Companies;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers
{
    public class CompaniesController : BaseController
    {
        public CompaniesController(IMediator mediator) : base(mediator)
        {
        }
        [HttpPost("AddCompany")]
        public async Task<IActionResult> AddCompany([FromBody]CompanyDto companyDto)
        {
            return await _mediator.Send(new AddCompanyCommand
            {
                CompanyDto = companyDto
            });
        }
    }
}

