using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Tapir.Core.Identity;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Application.Account.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ChangePasswordCommandResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ChangePasswordCommandResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.GetId();
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

            return new ChangePasswordCommandResult
            {
                Success = result.Succeeded,
                ErrorCode = result.Errors.Select(e => e.Code).FirstOrDefault()
            };
        }
    }
}
