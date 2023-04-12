using Application.CQRS.Offers.Commands;
using Application.CQRS.Offers.Queries.GetAllForCompany;
using Application.DTOs.Offers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Authorize]
public class OffersController : BaseController
{
    public OffersController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("GetAllForCompany")]
    public async Task<IActionResult> GetAllForCompany()
    {
        //Todo: testy integracyjne 
        var employeeId = HttpContext.User.FindFirst("EmployeeId");
        var result = await _mediator.Send(new GetAllForCompanyQuery
        {
            EmployeeId = Guid.Parse(employeeId.Value),
        });
        return Ok(result);
    }
    [HttpPost("AddOffer")]
    public async Task<IActionResult> AddOffer([FromBody]OfferDto offerDto)
    {
        //Todo: testy integracyjne negatywnej ścieżki
        var employeeId = HttpContext.User.FindFirst("EmployeeId");
        var result = await _mediator.Send(new AddOfferCommand
        {
            EmployeeId = Guid.Parse(employeeId.Value),
            OfferDto = offerDto
        });
        return Ok(result);
    }
    [AllowAnonymous]
    [HttpGet("GetOfferStatus/{offerId}")]
    public async Task<IActionResult> GetOfferStatus([FromRoute] Guid offerId)
    {
        return Ok(1);
    }
    
}