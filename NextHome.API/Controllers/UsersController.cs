using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextHome.Application.Auth.Responses;
using NextHome.Application.Auth.Commands;
using NextHome.Application.Users.Queries;
using NextHome.Application.Users.Commands;

namespace NextHome.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserResponse>> GetAll(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAllUsersRequest(), cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserResponse>> Update(
        [FromRoute] Guid id,    
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = request with { UserId = id };

        var response = await mediator.Send(command, cancellationToken);
        return Ok(response);
    }
}