using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tapir.Services.News.Application.Models;
using Tapir.Services.News.Application.Queries.GetNews;

namespace Tapir.Services.News.API.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMediator _mediator;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetNewsQuery());

        if (!result.IsValid)
        {
            return NotFound();
        }

        return new JsonResult(result.Value);
    }
}
