using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Tapir.Core.Queries;
using Tapir.Identity.Application.Account.Commands;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Tapir.Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        [HttpPost]
        [Route("info")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PagedResult<GetAccountQueryResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Info(GetAccountQuery query, [FromServices] IGetAccountQueryHandler handler)
        {
            return Ok(await handler.Process(query));
        }

        [HttpPost]
        [Route("change-username")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeUsername(ChangeUsernameCommand command, [FromServices] IChangeUsernameCommandHandler handler)
        {
            var result = await handler.Process(command);

            if (!result.Success)
            {
                return Problem(result.ErrorCode, null, StatusCodes.Status400BadRequest, "Failed to change username.");
            }

            return Ok();
        }

        [HttpPost]
        [Route("change-password")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command, [FromServices] IChangePasswordCommandHandler handler)
        {
            var result = await handler.Process(command);

            if (!result.Success)
            {
                return Problem(result.ErrorCode, null, StatusCodes.Status400BadRequest, "Failed to change password.");
            }

            return Ok();
        }

        [HttpPost]
        [Route("change-email")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeEmail(ChangeEmailCommand command, [FromServices] IChangeEmailCommandHandler handler)
        {
            var result = await handler.Process(command);

            if (!result.Success)
            {
                return Problem(result.ErrorCode, null, StatusCodes.Status400BadRequest, "Failed to change e-mail.");
            }

            return Ok();
        }

        [HttpPost]
        [Route("change-email/confirm")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmEmailChange(ConfirmEmailChangeCommand command, [FromServices] IConfirmEmailChangeCommandHandler handler)
        {
            var result = await handler.Process(command);

            if (!result.Success)
            {
                return Problem(result.ErrorCode, null, StatusCodes.Status400BadRequest, "Failed to confirm e-mail change.");
            }

            return Ok();
        }
    }
}
