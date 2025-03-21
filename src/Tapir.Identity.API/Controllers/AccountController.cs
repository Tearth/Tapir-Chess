using Microsoft.AspNetCore.Mvc;
using Tapir.Identity.Application.Auth.Services;

namespace Tapir.Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }
    }
}
