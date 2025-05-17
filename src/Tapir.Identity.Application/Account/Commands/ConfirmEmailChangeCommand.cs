using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using Tapir.Core.Commands;
using Tapir.Core.Messaging;
using Tapir.Core.Messaging.Identity;
using Tapir.Core.Validation;
using Tapir.Identity.Infrastructure.Commands;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Application.Account.Commands
{
    public class ConfirmEmailChangeCommand
    {
        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string UserId { get; set; }

        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string Token { get; set; }

        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string Email { get; set; }
    }

    public class ConfirmEmailChangeCommandResult : CommandResultBase<ConfirmEmailChangeCommandResult>
    {

    }

    public interface IConfirmEmailChangeCommandHandler : ICommandHandler<ConfirmEmailChangeCommand, ConfirmEmailChangeCommandResult>
    {

    }

    public class ConfirmEmailChangeCommandHandler : IConfirmEmailChangeCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMessageBus _messageBus;

        public ConfirmEmailChangeCommandHandler(UserManager<ApplicationUser> userManager, IMessageBus messageBus)
        {
            _userManager = userManager;
            _messageBus = messageBus;
        }

        public async Task<ConfirmEmailChangeCommandResult> Process(ConfirmEmailChangeCommand command, ClaimsPrincipal? user)
        {
            var userId = Encoding.UTF8.GetString(Convert.FromBase64String(command.UserId));
            var token = Encoding.UTF8.GetString(Convert.FromBase64String(command.Token));
            var email = Encoding.UTF8.GetString(Convert.FromBase64String(command.Email));
            var applicationUser = await _userManager.FindByIdAsync(userId);

            if (applicationUser == null)
            {
                return ConfirmEmailChangeCommandResult.Error("UserNotFound");
            }

            var result = await _userManager.ChangeEmailAsync(applicationUser, email, token);

            if (!result.Succeeded)
            {
                return new ConfirmEmailChangeCommandResult
                {
                    Success = result.Succeeded,
                    ErrorCode = result.Errors.Select(e => e.Code).FirstOrDefault()
                };
            }

            await _messageBus.Send(new UserUpdatedMessage
            {
                Id = applicationUser.Id,
                Username = applicationUser.UserName!,
                Email = email
            });

            return new ConfirmEmailChangeCommandResult
            {
                Success = true
            };
        }
    }
}
