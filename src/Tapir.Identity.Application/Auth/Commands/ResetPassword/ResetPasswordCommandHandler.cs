using MediatR;
using Microsoft.AspNetCore.Identity;
using Tapir.Core.Scheduler;
using Tapir.Identity.Application.Auth.Commands.RefreshToken;
using Tapir.Identity.Application.Auth.Mails.PasswordReset;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Application.Auth.Commands.ResetPassword
{
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

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _taskScheduler.Run(new PasswordResetMailTask
            {
                To = user.Email,
                UserId = user.Id.ToString(),
                Token = token
            });

            return new ResetPasswordCommandResult
            {
                Success = true
            };
        }
    }
}
