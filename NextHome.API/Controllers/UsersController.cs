using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextHome.Application.Auth.Responses;
using NextHome.Application.Auth.Commands;
using NextHome.Application.Users.Queries;

namespace NextHome.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    // TODO: uncomment it later when auth for swagger works
    // [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserResponse>> GetAll(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAllUsersRequest(), cancellationToken);
        return Ok(response);
    }
}