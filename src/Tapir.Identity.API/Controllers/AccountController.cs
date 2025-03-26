using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tapir.Core.Queries;
using Tapir.Identity.Application.Account.Commands.ChangePassword;
using Tapir.Identity.Application.Auth.Commands.Register;

namespace Tapir.Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("change-password")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PagedResult<RegisterCommandResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PagedResult<RegisterCommandResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
