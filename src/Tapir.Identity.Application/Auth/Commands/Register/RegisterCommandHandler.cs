using MediatR;
using Microsoft.AspNetCore.Identity;
using Tapir.Core.Scheduler;
using Tapir.Identity.Application.Auth.Mails.EmailConfirmation;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Application.Auth.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterCommandResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITaskScheduler _taskScheduler;

        public RegisterCommandHandler(UserManager<ApplicationUser> userManager, ITaskScheduler taskScheduler)
        {
            _userManager = userManager;
            _taskScheduler = taskScheduler;
        }

        public async Task<RegisterCommandResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new RegisterCommandResponse
                {
                    Success = result.Succeeded,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            await _userManager.AddToRoleAsync(user, "user");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            await _taskScheduler.Run(new EmailConfirmationMailTask
            {
                To = user.Email,
                UserId = user.Id.ToString(),
                Token = token
            });

            return new RegisterCommandResponse
            {
                Success = true
            };
        }
    }
}
