using System.Security.Claims;
using Application.CQRS.Companies.Commands;
using Application.CQRS.Companies.Commands.AddCompany;
using Application.CQRS.Companies.Commands.EditCompany;
using Application.CQRS.Companies.Queries.GetCompanyById;
using Application.CQRS.Companies.Queries.GetCompanyByOfferId;
using Application.DTOs;
using Application.DTOs.Companies;
using Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers
{
    [Route("companies")]
    public class CompaniesController : BaseController
    {
        public CompaniesController(IMediator mediator) : base(mediator)
        {
        }
        
        [HttpGet("getCompany")]
        [Authorize]
        public async Task<IActionResult> GetCompanyById()
        {
            BaseMessageDto baseMessageDto = await Mediator.Send(new GetCompanyByIdQuery
            {
                CompanyId = GetCompanyId()
            });
            if (baseMessageDto.IsSuccess())
                return Ok(baseMessageDto);
            else
                return BadRequest(baseMessageDto);
        }

        [HttpGet("getCompanyByOfferId/{offerId}")]
        public async Task<IActionResult> GetCompanyByOfferId([FromRoute] Guid offerId)
        {
            BaseMessageDto baseMessageDto = await Mediator.Send(new GetCompanyByOfferIdQuery {OfferId = offerId});
            if (baseMessageDto.IsSuccess())
                return Ok(baseMessageDto.Object);
            else
                return BadRequest(baseMessageDto);
        }
        
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

        [Authorize]
        [HttpPut("editCompany")]
        public async Task<IActionResult> EditCompany([FromBody] CompanyDto companyDto)
        {
            BaseMessageDto baseMessageDto = await Mediator.Send(new EditCompanyCommand
            {
                CompanyId = GetCompanyId(),
                CompanyDto = companyDto
            });
            if (baseMessageDto.IsSuccess())
                return Ok(baseMessageDto);
            else
                return BadRequest(baseMessageDto);
        }
    }
}

