using MediatR;
using Microsoft.AspNetCore.Mvc;
using NextHome.Application.Auth.Register;

namespace NextHome.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterCommand command)
    {
        var response = await mediator.Send(command);
        return Ok(response);
    }
}
