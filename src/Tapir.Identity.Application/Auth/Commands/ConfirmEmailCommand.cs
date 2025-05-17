using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tapir.Core.Commands;
using Tapir.Core.Validation;
using Tapir.Identity.Infrastructure.Commands;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Application.Auth.Commands
{
    public class ConfirmEmailCommand
    {
        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string UserId { get; set; }

        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string Token { get; set; }
    }

    public class ConfirmEmailCommandResult : CommandResultBase<ConfirmEmailCommandResult>
    {

    }

    public interface IConfirmEmailCommandHandler : ICommandHandler<ConfirmEmailCommand, ConfirmEmailCommandResult>
    {

    }

    public class ConfirmEmailCommandHandler : IConfirmEmailCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmEmailCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ConfirmEmailCommandResult> Process(ConfirmEmailCommand command)
        {
            var userId = Encoding.UTF8.GetString(Convert.FromBase64String(command.UserId));
            var token = Encoding.UTF8.GetString(Convert.FromBase64String(command.Token));
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return ConfirmEmailCommandResult.Error("UserNotFound");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            return new ConfirmEmailCommandResult
            {
                Success = result.Succeeded,
                ErrorCode = result.Errors.Select(e => e.Code).FirstOrDefault()
            };
        }
    }
}
