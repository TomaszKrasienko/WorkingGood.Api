using Application.CQRS.Offers.Commands;
using Application.CQRS.Offers.Commands.ChangeOfferStatus;
using Application.CQRS.Offers.Commands.DeleteOffer;
using Application.CQRS.Offers.Commands.EditOffer;
using Application.CQRS.Offers.Queries.GetOfferById;
using Application.CQRS.Offers.Queries.GetOffersList;
using Application.CQRS.Offers.Queries.GetOfferStatus;
using Application.CQRS.Offers.Queries.GetPositions;
using Application.DTOs.Offers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApi.Controllers;

[Authorize]
[Route("offers")]
public class OffersController : BaseController
{
    public OffersController(IMediator mediator) : base(mediator) { }
    
    [AllowAnonymous]
    [HttpGet("getOffersList")]
    public async Task<IActionResult> GetOffersList([FromQuery] GetOffersListRequestDto offersListRequestDto)
    {
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

    [HttpGet("getPositionsList")]
    public async Task<IActionResult> GetPositions()
    {
        var result = await Mediator.Send(new GetPositionsListQuery());
        return Ok(result);
    }
    
    
    [HttpPost("addOffer")]
    public async Task<IActionResult> AddOffer([FromBody]OfferDto offerDto)
    {
        Guid? employeeId = GetEmployeeId();
        var result = await Mediator.Send(new AddOfferCommand
        {
            EmployeeId = GetEmployeeId(),
            OfferDto = offerDto
        });
        if(result.IsSuccess())
            return Ok(result);
        return BadRequest(result);
    }
    
    [HttpPut("editOffer/{offerId}")]
    public async Task<IActionResult> EditOffer([FromRoute] Guid offerId, [FromBody] EditOfferRequestDto offerDto)
    {
        var result = await Mediator.Send(new EditOfferCommand
        {
            OfferId = offerId,
            OfferDto = offerDto
        });
        return Ok(result);
    }

    [HttpPatch("changeOfferStatus/{offerId}")]
    public async Task<IActionResult> ChangeOfferStatus([FromRoute] Guid offerId)
    {
        var result = await Mediator.Send(new ChangeOfferStatusCommand
        {
            OfferId = offerId
        });
        if(result.IsSuccess())
            return Ok(result);
        return BadRequest(result);
    }

    [HttpDelete("deleteOffer/{offerId}")]
    public async Task<IActionResult> DeleteOffer([FromRoute] Guid offerId)
    {
        var result = await Mediator.Send(new DeleteOfferCommand
        {
            OfferId = offerId
        });
        if(result.IsSuccess())
            return Ok(result);
        return BadRequest(result);
    }
}