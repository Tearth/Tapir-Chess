using MediatR;
using Microsoft.AspNetCore.Identity;
using Tapir.Core.Scheduler;
using Tapir.Identity.Application.Auth.Mails.EmailConfirmation;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Application.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterCommandResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITaskScheduler _taskScheduler;

        public RegisterCommandHandler(UserManager<ApplicationUser> userManager, ITaskScheduler taskScheduler)
        {
            _userManager = userManager;
            _taskScheduler = taskScheduler;
        }

        public async Task<RegisterCommandResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new RegisterCommandResult
                {
                    Success = result.Succeeded,
                    ErrorCode = result.Errors.Select(e => e.Code).FirstOrDefault()
                };
            }

            await _userManager.AddToRoleAsync(user, "user");
            await _taskScheduler.Run(new EmailConfirmationMailTask
            {
                To = user.Email,
                UserId = user.Id.ToString(),
                Token = await _userManager.GenerateEmailConfirmationTokenAsync(user)
            });

            return new RegisterCommandResult
            {
                Success = true
            };
        }
    }
}
