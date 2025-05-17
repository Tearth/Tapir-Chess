using Microsoft.AspNetCore.Mvc;
using Tapir.Core.Queries;
using Tapir.Services.News.Application.News.Commands;
using Tapir.Services.News.Application.News.Queries;
using Tapir.Services.News.Application.News.Queries.DTOs;

namespace Tapir.Services.News.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PagedResult<NewsDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int? pageNumber, int? pageSize, [FromServices] IGetNewsListQueryHandler handler)
        {
            var news = await handler.Process(new GetNewsListQuery
            {
                PageNumber = pageNumber ?? 1,
                PageSize = pageSize ?? 10
            }, User);

            return Ok(news);
        }

        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(NewsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id, [FromServices] IGetNewsQueryHandler handler)
        {
            var news = await handler.Process(new GetNewsQuery
            {
                Id = id
            }, User);

            if (news == null)
            {
                return NotFound();
            }

            return Ok(news);
        }

        [HttpPost]
        [Route("")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create(CreateNewsCommand command, [FromServices] ICreateNewsCommandHandler handler)
        {
            await handler.Process(command, User);
            return Ok();
        }

        [HttpPatch]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, UpdateNewsCommand command, [FromServices] IUpdateNewsCommandHandler handler)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await handler.Process(command, User);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, [FromServices] IDeleteNewsCommandHandler handler)
        {
            await handler.Process(new DeleteNewsCommand
            {
                Id = id
            }, User);
            return Ok();
        }
    }
}
