using Microsoft.AspNetCore.Mvc;
using Tapir.Core.Queries;
using Tapir.Services.News.Application.News.Commands;
using Tapir.Services.Players.Application.Players.Queries;
using Tapir.Services.Players.Application.Players.Queries.DTOs;

namespace Tapir.Services.Players.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayersController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PagedResult<PlayerDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int? pageNumber, int? pageSize, [FromServices] IGetPlayerListQueryHandler handler)
        {
            var players = await handler.Process(new GetPlayerListQuery
            {
                PageNumber = pageNumber ?? 1,
                PageSize = pageSize ?? 10
            });

            return Ok(players);
        }

        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PlayerDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id, [FromServices] IGetPlayerQueryHandler handler)
        {
            var player = await handler.Process(new GetPlayerQuery
            {
                Id = id
            });

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        [HttpPatch]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, UpdatePlayerCommand command, [FromServices] IUpdatePlayerCommandHandler handler)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await handler.Process(command);
            return Ok();
        }
    }
}
