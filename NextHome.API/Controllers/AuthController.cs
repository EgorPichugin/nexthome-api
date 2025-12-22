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
    public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterCommand command)
    {
        var response = await mediator.Send(command);
        return Ok(response);
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginCommand command)
    {
        try
        {
            var response = await mediator.Send(command);
            return Ok(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
