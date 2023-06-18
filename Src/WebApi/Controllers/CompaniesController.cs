using Application.CQRS.Companies.Commands;
using Application.CQRS.Companies.Commands.AddCompany;
using Application.DTOs;
using Application.DTOs.Companies;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers
{
    [Route("companies")]
    public class CompaniesController : BaseController
    {
        public CompaniesController(IMediator mediator) : base(mediator) { }
        [HttpPost("addCompany")]
        public async Task<IActionResult> AddCompany([FromBody]CompanyDto companyDto)
        {
            //Todo: Zwrócić jakieś Dto zamiast modelu
            BaseMessageDto baseMessageDto = await Mediator.Send(new AddCompanyCommand
                {
                    CompanyDto = companyDto
                });
            if (baseMessageDto.IsSuccess())
                return Ok(baseMessageDto);
            else
                return BadRequest(baseMessageDto);
        }


    }
}

