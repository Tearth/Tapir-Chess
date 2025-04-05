using Microsoft.AspNetCore.Mvc;
using Tapir.Core.Commands;
using Tapir.Core.Queries;
using Tapir.Identity.Application.Account.Commands;
using Tapir.Identity.Application.Auth.Commands;

namespace Tapir.Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        [Route("change-password")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PagedResult<RegisterCommandResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command, [FromServices] IChangePasswordCommandHandler handler)
        {
            var result = await handler.Process(command);

            if (!result.Success)
            {
                return Problem(result.ErrorCode, null, StatusCodes.Status400BadRequest, "Failed to change password.");
            }

            return Ok(result);
        }
    }
}
