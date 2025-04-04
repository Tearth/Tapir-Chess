using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tapir.Core.Queries;
using Tapir.Identity.Application.Auth.Commands.ConfirmEmail;
using Tapir.Identity.Application.Auth.Commands.ConfirmPassword;
using Tapir.Identity.Application.Auth.Commands.Login;
using Tapir.Identity.Application.Auth.Commands.RefreshToken;
using Tapir.Identity.Application.Auth.Commands.Register;
using Tapir.Identity.Application.Auth.Commands.ResetPassword;

namespace Tapir.Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public AuthController(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LogInCommand command)
        {
            var result = await _mediator.Send(command);
            
            if (!result.Success)
            {
                return Problem(result.ErrorCode, null, StatusCodes.Status400BadRequest, "Failed to authorize.");
            }

            SetCookies(result.AccessToken!, result.RefreshToken!);

            return Ok();
        }

        [HttpPost]
        [Route("register")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PagedResult<RegisterCommandResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PagedResult<ProblemDetails>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            
            if (!result.Success)
            {
                return Problem(result.ErrorCode, null, StatusCodes.Status400BadRequest, "Failed to complete registration.");
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("register/confirm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return Problem(result.ErrorCode, null, StatusCodes.Status400BadRequest, "Failed to confirm an email.");
            }

            return Ok();
        }


        [HttpPost]
        [Route("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return Problem(result.ErrorCode, null, StatusCodes.Status400BadRequest, "Failed to reset password.");
            }

            return Ok();
        }

        [HttpPost]
        [Route("reset-password/confirm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmPassword(ConfirmPasswordCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return Problem(result.ErrorCode, null, StatusCodes.Status400BadRequest, "Failed to confirm password.");
            }

            return Ok();
        }

        [HttpPost]
        [Route("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Headers["X-Refresh-Token"].FirstOrDefault();

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Problem("XRefreshTokenNotFound", null, StatusCodes.Status400BadRequest, "Failed to refresh token.");
            }

            var result = await _mediator.Send(new RefreshTokenCommand
            {
                RefreshToken = refreshToken
            });

            if (!result.Success)
            {
                return Problem(result.ErrorCode, null, StatusCodes.Status400BadRequest, "Failed to refresh token.");
            }

            SetCookies(result.AccessToken!, result.RefreshToken!);

            return Ok();
        }

        private void SetCookies(string accessToken, string refreshToken)
        {
            Response.Cookies.Append("access_token", accessToken, new CookieOptions
            {
                SameSite = SameSiteMode.Strict,
                HttpOnly = true,
                Secure = true
            });

            Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            {
                Path = "/api/auth/refresh-token",
                SameSite = SameSiteMode.Strict,
                HttpOnly = true,
                Secure = true
            });
        }
    }
}
