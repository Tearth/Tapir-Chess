using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Tapir.Core.Commands;
using Tapir.Core.Identity;
using Tapir.Core.Validation;
using Tapir.Identity.Infrastructure.Commands;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Application.Account.Commands
{
    public class ChangePasswordCommand
    {
        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string OldPassword { get; set; }

        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string NewPassword { get; set; }
    }

    public class ChangePasswordCommandResult : CommandResultBase<ChangePasswordCommandResult>
    {

    }

    public interface IChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, ChangePasswordCommandResult>
    {

    }

    public class ChangePasswordCommandHandler : IChangePasswordCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ChangePasswordCommandResult> Process(ChangePasswordCommand command)
        {
            var userId = _httpContextAccessor.HttpContext?.User.GetId();
            if (userId == null)
            {
                return ChangePasswordCommandResult.Error("UserNotFound");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return ChangePasswordCommandResult.Error("UserNotFound");
            }

            var result = await _userManager.ChangePasswordAsync(user, command.OldPassword, command.NewPassword);
            return new ChangePasswordCommandResult
            {
                Success = result.Succeeded,
                ErrorCode = result.Errors.Select(e => e.Code).FirstOrDefault()
            };
        }
    }
}
