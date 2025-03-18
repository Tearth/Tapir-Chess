using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tapir.Core.Queries;
using Tapir.Services.News.Application.News.Commands;
using Tapir.Services.News.Application.News.Queries;
using Tapir.Services.News.Application.News.Queries.DTOs;

namespace Tapir.Services.News.API.Controllers
{
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
        [Route("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PagedResult<NewsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 10)
        {
            var news = await _mediator.Send(new GetNewsListQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });

            return Ok(news);
        }

        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(NewsDto), StatusCodes.Status200OK)]
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
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateNewsCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPatch]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _mediator.Send(new DeleteNewsCommand
            {
                Id = id
            });
            return Ok();
        }
    }
}
