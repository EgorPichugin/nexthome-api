using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextHome.Application.Auth.Responses;
using NextHome.Application.Countries.Queries;
using NextHome.Application.Countries.Responses;

namespace NextHome.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CountryResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetAllCountries(), cancellationToken);
        return Ok(response);
    }
}