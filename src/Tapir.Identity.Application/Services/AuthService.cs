using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;

namespace Tapir.Identity.Application.Services
{
    public class AuthService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AuthService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
    }
}
