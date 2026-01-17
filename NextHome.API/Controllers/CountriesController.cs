using MediatR;
using Microsoft.AspNetCore.Mvc;
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
        var response = await mediator.Send(new GetAllCountriesQuery(), cancellationToken);
        return Ok(response);
    }
}