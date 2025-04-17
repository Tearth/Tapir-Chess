using Microsoft.AspNetCore.Mvc;
using Tapir.Core.Queries;
using Tapir.Identity.Application.Auth.Commands;
using Tapir.Identity.Infrastructure;

namespace Tapir.Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppSettings _settings;

        public AuthController(AppSettings settings)
        {
            _settings = settings;
        }

        [HttpPost]
        [Route("signin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignIn(SignInCommand command, [FromServices] ISignInCommandHandler handler)
        {
            var result = await handler.Process(command);
            
            if (!result.Success)
            {
                return Problem(result.ErrorCode, null, StatusCodes.Status400BadRequest, "Failed to authorize.");
            }

            SetCookies(result.AccessToken!, result.RefreshToken!, command.RememberMe);

            return Ok();
        }

        [HttpPost]
        [Route("register")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PagedResult<RegisterCommandResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PagedResult<ProblemDetails>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterCommand command, [FromServices] IRegisterCommandHandler handler)
        {
            var result = await handler.Process(command);
            
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
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command, [FromServices] IConfirmEmailCommandHandler handler)
        {
            var result = await handler.Process(command);

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
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command, [FromServices] IResetPasswordCommandHandler handler)
        {
            var result = await handler.Process(command);

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
        public async Task<IActionResult> ConfirmPassword(ConfirmPasswordCommand command, [FromServices] IConfirmPasswordCommandHandler handler)
        {
            var result = await handler.Process(command);

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
        public async Task<IActionResult> RefreshToken([FromServices] IRefreshTokenCommandHandler handler)
        {
            var refreshToken = Request.Headers["X-Refresh-Token"].FirstOrDefault();
            var rememberMe = Request.Headers["X-Remember-Me"].FirstOrDefault();

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Problem("XRefreshTokenNotFound", null, StatusCodes.Status400BadRequest, "Failed to refresh token.");
            }

            var result = await handler.Process(new RefreshTokenCommand
            {
                RefreshToken = refreshToken
            });

            if (!result.Success)
            {
                return Problem(result.ErrorCode, null, StatusCodes.Status400BadRequest, "Failed to refresh token.");
            }

            SetCookies(result.AccessToken!, result.RefreshToken!, rememberMe == "true");

            return Ok();
        }

        private void SetCookies(string accessToken, string refreshToken, bool rememberMe)
        {
            Response.Cookies.Append("access_token", accessToken, new CookieOptions
            {
                SameSite = SameSiteMode.Strict,
                HttpOnly = true,
                Secure = true,
                MaxAge = TimeSpan.FromMinutes(_settings.Jwt.AccessTokenExpirationTime)
            });

            Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
            {
                Path = "/api/auth/refresh-token",
                SameSite = SameSiteMode.Strict,
                HttpOnly = true,
                Secure = true,
                MaxAge = rememberMe ? TimeSpan.FromMinutes(_settings.Jwt.RefreshTokenExpirationTime) : null
            });

            if (rememberMe)
            {
                Response.Cookies.Append("remember_me", "true", new CookieOptions
                {
                    SameSite = SameSiteMode.Strict,
                    HttpOnly = true,
                    Secure = true,
                    MaxAge = TimeSpan.FromMinutes(_settings.Jwt.RefreshTokenExpirationTime)
                });
            }
            else
            {
                Response.Cookies.Delete("remember_me");
            }
        }
    }
}
