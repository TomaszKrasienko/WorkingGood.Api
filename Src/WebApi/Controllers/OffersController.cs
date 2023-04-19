using System.Security.Claims;
using Application.CQRS.Offers.Commands;
using Application.CQRS.Offers.Queries.GetAllForCompany;
using Application.CQRS.Offers.Queries.GetById;
using Application.CQRS.Offers.Queries.GetOfferStatus;
using Application.DTOs.Offers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Common.Exceptions;

namespace WebApi.Controllers;

[Authorize]
[Route("offers")]
public class OffersController : BaseController
{
    public OffersController(IMediator mediator) : base(mediator)
    {
    }
    [HttpGet("getAllForCompany")]
    public async Task<IActionResult> GetAllForCompany()
    {
        //Todo: testy integracyjne 
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var employeeId = identity!.FindFirst(EMPLOYEE_ID_KEY)?.Value ?? throw new UserNotFoundException();
        var result = await Mediator.Send(new GetAllForCompanyQuery
        {
            EmployeeId = Guid.Parse(employeeId),
        });
        return Ok(result);
    }
    [AllowAnonymous]
    [HttpGet("getOfferStatus/{offerId}")]
    public async Task<IActionResult> GetOfferStatus([FromRoute] Guid offerId)
    {
        var result = await Mediator.Send(new GetStatusQuery
        {
            OfferId = offerId
        });
        return Ok(result);
    }
    [AllowAnonymous]
    [HttpGet("getById/{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        //Todo: testy integracyjne
        var result = await Mediator.Send(new GetByIdQuery
        {
            Id = id
        });
        return Ok(result);
    }
    [HttpPost("addOffer")]
    public async Task<IActionResult> AddOffer([FromBody]OfferDto offerDto)
    {
        //Todo: testy integracyjne negatywnej ścieżki
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var employeeId = HttpContext.User.FindFirst(EMPLOYEE_ID_KEY)!.Value ?? throw new UserNotFoundException();
        var result = await Mediator.Send(new AddOfferCommand
        {
            EmployeeId = Guid.Parse(employeeId),
            OfferDto = offerDto
        });
        return Ok(result);
    }
}