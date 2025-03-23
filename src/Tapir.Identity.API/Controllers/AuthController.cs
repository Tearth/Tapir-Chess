using Microsoft.AspNetCore.Mvc;
using System.Text;
using Tapir.Core.Queries;
using Tapir.Identity.Application.Auth.Requests;
using Tapir.Identity.Application.Auth.Responses;
using Tapir.Identity.Application.Auth.Services;

namespace Tapir.Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authService.Login(request);
            
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
        [ProducesResponseType(typeof(PagedResult<RegisterResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PagedResult<RegisterResponse>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authService.Register(request);
            
            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("register/confirm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _authService.ConfirmEmail(
                Encoding.UTF8.GetString(Convert.FromBase64String(userId)),
                Encoding.UTF8.GetString(Convert.FromBase64String(token))
            );

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

            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest();
            }

            var result = await _authService.RefreshToken(new RefreshTokenRequest
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
