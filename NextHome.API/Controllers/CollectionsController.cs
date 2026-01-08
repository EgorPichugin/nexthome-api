using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextHome.Application.Qdrant.Commands;
using NextHome.Application.Qdrant.Queries;
using NextHome.Application.Users.Responses;
using NextHome.Core.Entities;
using Qdrant.Client.Grpc;

namespace NextHome.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CollectionsController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPut("{collectionName?}")]
    public async Task<IActionResult> CreateCollection(
        [FromRoute] string? collectionName,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new CreateCollectionCommand(collectionName), cancellationToken);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{collectionName}")]
    public async Task<IActionResult> DeleteCollection(
        [FromRoute] string collectionName,
        CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteCollectionCommand(collectionName), cancellationToken);
        return NoContent();   
    }

    [Authorize]
    [HttpGet]
    public async Task<List<string>> GetCollectionList(CancellationToken cancellationToken)
    {
        return await mediator.Send(new GetCollectionListQuery(), cancellationToken);
    }

    [Authorize]
    [HttpPost("cards/similar")]
    public async Task<ActionResult<List<ExperienceCardResponse>>> GetSimilarExperience(
        [FromBody] GetSimilarExperienceCardsRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetSimilarExperienceCardsCommand(request), cancellationToken);
        return Ok(response);
    }
}