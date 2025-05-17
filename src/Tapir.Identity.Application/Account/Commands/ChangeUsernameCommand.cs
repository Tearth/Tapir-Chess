using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Tapir.Core.Commands;
using Tapir.Core.Identity;
using Tapir.Core.Messaging;
using Tapir.Core.Messaging.Identity;
using Tapir.Core.Validation;
using Tapir.Identity.Infrastructure.Commands;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Application.Account.Commands
{
    public class ChangeUsernameCommand
    {
        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string Username { get; set; }
    }

    public class ChangeUsernameCommandResult : CommandResultBase<ChangeUsernameCommandResult>
    {

    }

    public interface IChangeUsernameCommandHandler : ICommandHandler<ChangeUsernameCommand, ChangeUsernameCommandResult>
    {

    }

    public class ChangeUsernameCommandHandler : IChangeUsernameCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMessageBus _messageBus;

        public ChangeUsernameCommandHandler(UserManager<ApplicationUser> userManager, IMessageBus messageBus)
        {
            _userManager = userManager;
            _messageBus = messageBus;
        }

        public async Task<ChangeUsernameCommandResult> Process(ChangeUsernameCommand command, ClaimsPrincipal? user)
        {
            var userId = user.GetId();
            if (userId == null)
            {
                return ChangeUsernameCommandResult.Error("UserNotFound");
            }

            var applicationUser = await _userManager.FindByIdAsync(userId.ToString());
            if (applicationUser == null)
            {
                return ChangeUsernameCommandResult.Error("UserNotFound");
            }

            var result = await _userManager.SetUserNameAsync(applicationUser, command.Username);

            if (!result.Succeeded)
            {
                return new ChangeUsernameCommandResult
                {
                    Success = result.Succeeded,
                    ErrorCode = result.Errors.Select(e => e.Code).FirstOrDefault()
                };
            }

            await _messageBus.Send(new UserUpdatedMessage
            {
                Id = applicationUser.Id,
                Username = command.Username,
                Email = applicationUser.Email!
            });

            return new ChangeUsernameCommandResult
            {
                Success = true
            };
        }
    }
}
