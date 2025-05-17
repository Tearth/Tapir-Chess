using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Tapir.Core.Commands;
using Tapir.Core.Identity;
using Tapir.Core.Scheduler;
using Tapir.Core.Validation;
using Tapir.Core.Validation.Attributes;
using Tapir.Identity.Application.Auth.Mails.EmailChange;
using Tapir.Identity.Application.Auth.Mails.PasswordReset;
using Tapir.Identity.Infrastructure.Commands;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Application.Account.Commands
{
    public class ChangeEmailCommand
    {
        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        [Email(ErrorMessage = ValidationErrorCodes.INVALID_EMAIL)]
        public required string Email { get; set; }
    }

    public class ChangeEmailCommandResult : CommandResultBase<ChangeEmailCommandResult>
    {

    }

    public interface IChangeEmailCommandHandler : ICommandHandler<ChangeEmailCommand, ChangeEmailCommandResult>
    {

    }

    public class ChangeEmailCommandHandler : IChangeEmailCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITaskScheduler _taskScheduler;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChangeEmailCommandHandler(UserManager<ApplicationUser> userManager, ITaskScheduler taskScheduler, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _taskScheduler = taskScheduler;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ChangeEmailCommandResult> Process(ChangeEmailCommand request)
        {
            var userId = _httpContextAccessor.HttpContext?.User.GetId();
            if (userId == null)
            {
                return ChangeEmailCommandResult.Error("UserNotFound");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return ChangeEmailCommandResult.Error("UserNotFound");
            }

            await _taskScheduler.Run(new EmailChangeMailTask
            {
                To = user.Email!,
                UserId = user.Id,
                Token = await _userManager.GenerateChangeEmailTokenAsync(user, request.Email),
                NewEmail = request.Email,
            });

            return new ChangeEmailCommandResult
            {
                Success = true
            };
        }
    }
}
