using MediatR;
using Microsoft.AspNetCore.Identity;
using Tapir.Core.Scheduler;
using Tapir.Identity.Application.Auth.Mails.PasswordReset;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Application.Auth.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITaskScheduler _taskScheduler;

        public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager, ITaskScheduler taskScheduler)
        {
            _userManager = userManager;
            _taskScheduler = taskScheduler;
        }

        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _taskScheduler.Run(new PasswordResetMailTask
            {
                To = user.Email,
                UserId = user.Id.ToString(),
                Token = token
            });

            return true;
        }
    }
}
