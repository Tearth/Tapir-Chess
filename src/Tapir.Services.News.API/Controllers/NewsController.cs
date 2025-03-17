using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tapir.Services.News.Application.News.Commands;
using Tapir.Services.News.Application.News.Queries;

namespace Tapir.Services.News.API.Controllers;

[ApiController]
[Route("[controller]")]
public class NewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public NewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var news = await _mediator.Send(new GetNewsQuery
        {
            Id = id
        });

        if (news == null)
        {
            return NotFound();
        }

        return Ok(news);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNewsCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateNewsCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await _mediator.Send(command);
        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteNewsCommand
        {
            Id = id
        });
        return Ok();
    }
}
