using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Tapir.Core.Commands;
using Tapir.Core.Scheduler;
using Tapir.Core.Validation;
using Tapir.Core.Validation.Attributes;
using Tapir.Identity.Application.Auth.Mails.PasswordReset;
using Tapir.Identity.Infrastructure.Commands;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Application.Auth.Commands
{
    public class ResetPasswordCommand
    {
        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        [Email(ErrorMessage = ValidationErrorCodes.INVALID_EMAIL)]
        public required string Email { get; set; }
    }

    public class ResetPasswordCommandResult : CommandResultBase<ResetPasswordCommandResult>
    {

    }

    public interface IResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, ResetPasswordCommandResult>
    {

    }

    public class ResetPasswordCommandHandler : IResetPasswordCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITaskScheduler _taskScheduler;

        public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager, ITaskScheduler taskScheduler)
        {
            _userManager = userManager;
            _taskScheduler = taskScheduler;
        }

        public async Task<ResetPasswordCommandResult> Process(ResetPasswordCommand command, ClaimsPrincipal? user)
        {
            var applicationUser = await _userManager.FindByEmailAsync(command.Email);

            if (applicationUser == null)
            {
                return ResetPasswordCommandResult.Error("UserNotFound");
            }

            await _taskScheduler.Run(new PasswordResetMailTask
            {
                To = applicationUser.Email!,
                UserId = applicationUser.Id,
                Token = await _userManager.GeneratePasswordResetTokenAsync(applicationUser)
            });

            return new ResetPasswordCommandResult
            {
                Success = true
            };
        }
    }
}
