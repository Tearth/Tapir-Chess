using Microsoft.AspNetCore.Mvc;
using Tapir.Core.Queries;
using Tapir.Services.Games.Application.Rooms.Commands;
using Tapir.Services.Games.Application.Rooms.Queries;
using Tapir.Services.Games.Application.Rooms.Queries.DTOs;

namespace Tapir.Services.Games.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PagedResult<RoomDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int? pageNumber, int? pageSize, [FromServices] IGetRoomListQueryHandler handler)
        {
            var rooms = await handler.Process(new GetRoomListQuery
            {
                PageNumber = pageNumber ?? 1,
                PageSize = pageSize ?? 10
            }, User);

            return Ok(rooms);
        }

        [HttpGet]
        [Route("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(RoomDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id, [FromServices] IGetRoomQueryHandler handler)
        {
            var room = await handler.Process(new GetRoomQuery
            {
                Id = id
            }, User);

            if (room == null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        [HttpPost]
        [Route("")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create(CreateRoomCommand command, [FromServices] ICreateRoomCommandHandler handler)
        {
            await handler.Process(command, User);
            return Ok();
        }
    }
}
