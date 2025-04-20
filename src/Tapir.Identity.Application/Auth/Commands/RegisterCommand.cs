using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Tapir.Core.Commands;
using Tapir.Core.Messaging;
using Tapir.Core.Messaging.Identity;
using Tapir.Core.Scheduler;
using Tapir.Core.Validation;
using Tapir.Core.Validation.Attributes;
using Tapir.Identity.Application.Auth.Mails.EmailConfirmation;
using Tapir.Identity.Infrastructure.Commands;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Application.Auth.Commands
{
    public class RegisterCommand
    {
        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string Username { get; set; }

        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string Password { get; set; }

        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        [Email(ErrorMessage = ValidationErrorCodes.INVALID_EMAIL)]
        public required string Email { get; set; }
    }

    public class RegisterCommandResult : CommandResultBase<RegisterCommandResult>
    {

    }

    public interface IRegisterCommandHandler : ICommandHandler<RegisterCommand, RegisterCommandResult>
    {

    }

    public class RegisterCommandHandler : IRegisterCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMessageBus _messageBus;
        private readonly ITaskScheduler _taskScheduler;

        public RegisterCommandHandler(UserManager<ApplicationUser> userManager, IMessageBus messageBus, ITaskScheduler taskScheduler)
        {
            _userManager = userManager;
            _messageBus = messageBus;
            _taskScheduler = taskScheduler;
        }

        public async Task<RegisterCommandResult> Process(RegisterCommand request)
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

            await _messageBus.Send(new UserCreatedMessage
            {
                Id = user.Id,
                Username = request.Username,
                Email = request.Email
            });

            return new RegisterCommandResult
            {
                Success = true
            };
        }
    }
}
