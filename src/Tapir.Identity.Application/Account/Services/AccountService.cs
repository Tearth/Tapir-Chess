using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Tapir.Core.Identity;
using Tapir.Identity.Application.Account.Requests;
using Tapir.Identity.Application.Account.Responses;
using Tapir.Identity.Application.Auth.Responses;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Application.Account.Services
{
    public class AccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.OldPassword) || string.IsNullOrEmpty(request.NewPassword))
            {
                return new ChangePasswordResponse
                {
                    Success = false
                };
            }

            var userId = _httpContextAccessor.HttpContext?.User.GetId();
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            
            return new ChangePasswordResponse
            {
                Success = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }
    }
}
