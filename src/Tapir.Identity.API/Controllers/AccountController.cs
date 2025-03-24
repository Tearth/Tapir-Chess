using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tapir.Core.Queries;
using Tapir.Identity.Application.Account.Requests;
using Tapir.Identity.Application.Account.Services;
using Tapir.Identity.Application.Auth.Responses;
using Tapir.Identity.Application.Auth.Services;

namespace Tapir.Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Route("change-password")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PagedResult<RegisterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PagedResult<RegisterResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            var result = await _accountService.ChangePassword(request);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
