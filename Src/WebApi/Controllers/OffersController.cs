using System.Security.Claims;
using Application.CQRS.Offers.Commands;
using Application.CQRS.Offers.Queries.GetActiveOffers;
using Application.CQRS.Offers.Queries.GetAllForCompany;
using Application.CQRS.Offers.Queries.GetById;
using Application.CQRS.Offers.Queries.GetOfferById;
using Application.CQRS.Offers.Queries.GetOffersList;
using Application.CQRS.Offers.Queries.GetOfferStatus;
using Application.DTOs.Offers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
        //string? employeeId = null;
        var result = await Mediator.Send(new GetOffersListQuery()
        {
            GetOffersListRequestDto = offersListRequestDto,
            EmployeeId = GetEmployeeId()
        });
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.MetaData));
        return Ok(result.Object);
    }

    [HttpGet("getOfferById/{offerId}")]
    public async Task<IActionResult> GetOfferById([FromRoute] Guid offerId)
    {
        var result = await Mediator.Send(new GetOfferByIdCommand
        {
            OfferId = offerId
        });
        if (result.IsSuccess())
            return Ok(result.Object);
        return BadRequest(result.Errors);
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
        var result = await Mediator.Send(new AddOfferCommand
        {
            EmployeeId = GetEmployeeId(),
            OfferDto = offerDto
        });
        return Ok(result);
    }
    [HttpPut("editOffer/{id}")]
    public async Task<IActionResult> EditOffer([FromRoute] string offerId, [FromBody] OfferDto offerDto)
    {
        return Ok();
    }
}