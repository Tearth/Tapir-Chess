using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tapir.Services.News.Application.News.Commands;

namespace Tapir.Services.News.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IMediator _mediator;

    public WeatherForecastController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await _mediator.Send(new AddNewsCommand
        {
           
        });

        return Ok();
    }
}
