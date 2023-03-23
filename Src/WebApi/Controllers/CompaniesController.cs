using Application.CQRS.Companies.Commands;
using Application.DTOs.Companies;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers
{
    public class CompaniesController : BaseController
    {
        private readonly ILogger<CompaniesController> _logger;
        public CompaniesController(ILogger<CompaniesController> logger, IMediator mediator) : base(mediator)
        {
            _logger = logger;
        }
        [HttpPost("AddCompany")]
        public async Task<IActionResult> AddCompany([FromBody]CompanyDto companyDto)
        {
            return await _mediator.Send(new AddCompanyCommand
            {
                CompanyDto = companyDto
            });
        }

        [HttpPost]
        public async Task<IActionResult> Test()
        {
            try
            {
                throw new Exception();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Ok();
        }
    }
}

