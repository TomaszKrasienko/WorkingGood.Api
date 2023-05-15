using System.Security.Claims;
using Application.CQRS.Offers.Commands;
using Application.CQRS.Offers.Queries.GetActiveOffers;
using Application.CQRS.Offers.Queries.GetAllForCompany;
using Application.CQRS.Offers.Queries.GetById;
using Application.CQRS.Offers.Queries.GetOffersList;
using Application.CQRS.Offers.Queries.GetOfferStatus;
using Application.DTOs.Offers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApi.Common.Exceptions;

namespace WebApi.Controllers;

[Authorize]
[Route("offers")]
public class OffersController : BaseController
{
    public OffersController(IMediator mediator) : base(mediator)
    {
    }

    [AllowAnonymous]
    [HttpGet("getOffersList")]
    public async Task<IActionResult> GetOffersList([FromQuery] GetOffersListRequestDto offersListRequestDto)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var employeeId = identity!.FindFirst(EMPLOYEE_ID_KEY)?.Value ?? null;
        //string? employeeId = null;
        var result = await Mediator.Send(new GetOffersListQuery()
        {
            GetOffersListRequestDto = offersListRequestDto,
            EmployeeId = employeeId == null ? null : Guid.Parse(employeeId)
        });
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));
        return Ok(result.Object);
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
    [HttpGet("getActiveById/{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        //Todo: testy integracyjne
        var result = await Mediator.Send(new GetActiveOfferByIdQuery
        {
            Id = id
        });
        return Ok(result);
    }
    // [HttpGet("getAllForCompany")]
    // public async Task<IActionResult> GetAllForCompany()
    // {
    //     //Todo: testy integracyjne 
    //     var identity = HttpContext.User.Identity as ClaimsIdentity;
    //     var employeeId = identity!.FindFirst(EMPLOYEE_ID_KEY)?.Value ?? throw new UserNotFoundException();
    //     var result = await Mediator.Send(new GetAllForCompanyQuery
    //     {
    //         EmployeeId = Guid.Parse(employeeId),
    //     });
    //     return Ok(result);
    // }
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