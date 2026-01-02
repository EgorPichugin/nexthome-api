using MediatR;
using Microsoft.AspNetCore.Mvc;
using NextHome.Application.Auth.Responses;
using NextHome.Application.Auth.Commands;

namespace NextHome.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new RegisterUserCommand(request), cancellationToken);
        return Ok(response);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResponse>> Login([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new LoginUserCommand(request), cancellationToken);
        return Ok(response);
    }
}
