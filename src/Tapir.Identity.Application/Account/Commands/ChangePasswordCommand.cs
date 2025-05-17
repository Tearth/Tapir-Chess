using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
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

        public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ChangePasswordCommandResult> Process(ChangePasswordCommand command, ClaimsPrincipal? user)
        {
            var userId = user.GetId();
            if (userId == null)
            {
                return ChangePasswordCommandResult.Error("UserNotFound");
            }

            var applicationUser = await _userManager.FindByIdAsync(userId.ToString());
            if (applicationUser == null)
            {
                return ChangePasswordCommandResult.Error("UserNotFound");
            }

            var result = await _userManager.ChangePasswordAsync(applicationUser, command.OldPassword, command.NewPassword);
            return new ChangePasswordCommandResult
            {
                Success = result.Succeeded,
                ErrorCode = result.Errors.Select(e => e.Code).FirstOrDefault()
            };
        }
    }
}
