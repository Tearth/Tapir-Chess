using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Tapir.Core.Scheduler;
using Tapir.Core.Validation;
using Tapir.Identity.Application.Auth.Mails.PasswordReset;
using Tapir.Identity.Infrastructure.Commands;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Application.Auth.Commands
{
    public class ResetPasswordCommand : IRequest<ResetPasswordCommandResult>
    {
        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        [EmailAddress(ErrorMessage = ValidationErrorCodes.INVALID_EMAIL)]
        public required string Email { get; set; }
    }

    public class ResetPasswordCommandResult : CommandResultBase<ResetPasswordCommandResult>
    {

    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordCommandResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITaskScheduler _taskScheduler;

        public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager, ITaskScheduler taskScheduler)
        {
            _userManager = userManager;
            _taskScheduler = taskScheduler;
        }

        public async Task<ResetPasswordCommandResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return ResetPasswordCommandResult.Error("UserNotFound");
            }

            await _taskScheduler.Run(new PasswordResetMailTask
            {
                To = user.Email!,
                UserId = user.Id.ToString(),
                Token = await _userManager.GeneratePasswordResetTokenAsync(user)
            });

            return new ResetPasswordCommandResult
            {
                Success = true
            };
        }
    }
}
