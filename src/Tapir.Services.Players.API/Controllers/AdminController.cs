using Microsoft.AspNetCore.Mvc;
using Tapir.Services.Players.Application.Admin.Commands;

namespace Tapir.Services.Players.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        [HttpPost]
        [Route("rebuild-read-model")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RebuildReadModel(RebuildReadModelCommand command, [FromServices] IRebuildReadModelCommandHandler handler)
        {
            await handler.Process(command, User);
            return Ok();
        }
    }
}
