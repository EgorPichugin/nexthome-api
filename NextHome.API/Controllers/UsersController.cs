using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextHome.Application.Auth.Responses;
using NextHome.Application.Users.Queries;
using NextHome.Application.Users.Commands;
using NextHome.Application.Users.Responses;

namespace NextHome.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserResponse>> GetAll(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAllUsersQuery(), cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserResponse>> Update(
        [FromRoute] Guid id,    
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateUserCommand(id, request), cancellationToken);
        return Ok(response);
    }
    
    //region Experience cards
    [Authorize]
    [HttpPost("{id:guid}/cards/experience")]
    public async Task<ActionResult<List<ExperienceCardResponse>>> CreateExperienceCard(
        [FromRoute] Guid id,
        [FromBody] CreateExperienceCardRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateExperienceCardCommand(id, request), cancellationToken);
        return Ok(response);
    }
    
    [Authorize]
    [HttpGet("{id:guid}/cards/experience")]
    public async Task<ActionResult<List<ExperienceCardResponse>>> GetExperienceCards(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetExperienceCardsQuery(id), cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpPut("{id:guid}/cards/experience/{cardId:guid}")]
    public async Task<ActionResult<ExperienceCardResponse>> UpdateExperienceCard(
        [FromRoute] Guid id,
        [FromRoute] Guid cardId,
        [FromBody] UpdateExperienceCardRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new UpdateExperienceCardCommand(id, cardId, request), cancellationToken);
        return Ok(response);
    }
    
    [Authorize]
    [HttpDelete("{id:guid}/cards/experience/{cardId:guid}")]
    public async Task<IActionResult> DeleteExperienceCard(
        [FromRoute] Guid id,
        [FromRoute] Guid cardId,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new DeleteExperienceCardCommand(id, cardId), cancellationToken);
        return NoContent();
    }
    //endregion
}