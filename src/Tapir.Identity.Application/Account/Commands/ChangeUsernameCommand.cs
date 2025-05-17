using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChangeUsernameCommandHandler(UserManager<ApplicationUser> userManager, IMessageBus messageBus, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _messageBus = messageBus;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ChangeUsernameCommandResult> Process(ChangeUsernameCommand command)
        {
            var userId = _httpContextAccessor.HttpContext?.User.GetId();
            if (userId == null)
            {
                return ChangeUsernameCommandResult.Error("UserNotFound");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return ChangeUsernameCommandResult.Error("UserNotFound");
            }

            var result = await _userManager.SetUserNameAsync(user, command.Username);

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
                Id = user.Id,
                Username = command.Username,
                Email = user.Email!
            });

            return new ChangeUsernameCommandResult
            {
                Success = true
            };
        }
    }
}
