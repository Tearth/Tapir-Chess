using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Tapir.Core.Queries;
using Tapir.Identity.Application.Auth.Commands;
using Tapir.Identity.Application.Auth.Commands.ConfirmEmail;
using Tapir.Identity.Application.Auth.Commands.ConfirmPassword;
using Tapir.Identity.Application.Auth.Commands.Login;
using Tapir.Identity.Application.Auth.Commands.RefreshToken;
using Tapir.Identity.Application.Auth.Commands.Register;

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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LogInCommand command)
        {
            var result = await _mediator.Send(command);
            
            if (!result.Success)
            {
                return BadRequest();
            }

            Response.Cookies.Append("access_token", result.AccessToken, new CookieOptions
            {
                SameSite = SameSiteMode.Strict,
                HttpOnly = true,
                Secure = true
            });
            Response.Cookies.Append("refresh_token", result.RefreshToken, new CookieOptions
            {
                Path = "/api/auth/refresh-token",
                SameSite = SameSiteMode.Strict,
                HttpOnly = true,
                Secure = true
            });

            return Ok();
        }

        [HttpPost]
        [Route("register")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PagedResult<RegisterCommandResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PagedResult<RegisterCommandResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("register/confirm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }


        [HttpPost]
        [Route("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [Route("reset-password/confirm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmPassword(ConfirmPasswordCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost]
        [Route("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Headers["X-Refresh-Token"].FirstOrDefault();

            var result = await _mediator.Send(new RefreshTokenCommand
            {
                RefreshToken = refreshToken
            });

            if (!result.Success)
            {
                return BadRequest();
            }

            Response.Cookies.Append("access_token", result.AccessToken, new CookieOptions
            {
                SameSite = SameSiteMode.Strict,
                HttpOnly = true,
                Secure = true
            });
            Response.Cookies.Append("refresh_token", result.RefreshToken, new CookieOptions
            {
                Path = "/api/auth/refresh-token",
                SameSite = SameSiteMode.Strict,
                HttpOnly = true,
                Secure = true
            });

            return Ok(result);
        }
    }
}
