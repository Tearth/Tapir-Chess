﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tapir.Core.Commands;
using Tapir.Core.Validation;
using Tapir.Identity.Infrastructure.Commands;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Application.Auth.Commands
{
    public class ConfirmPasswordCommand
    {
        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string UserId { get; set; }

        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string Token { get; set; }

        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string Password { get; set; }
    }

    public class ConfirmPasswordCommandResult : CommandResultBase<ConfirmPasswordCommandResult>
    {

    }

    public interface IConfirmPasswordCommandHandler : ICommandHandler<ConfirmPasswordCommand, ConfirmPasswordCommandResult>
    {

    }

    public class ConfirmPasswordCommandHandler : IConfirmPasswordCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmPasswordCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ConfirmPasswordCommandResult> Process(ConfirmPasswordCommand request)
        {
            var userId = Encoding.UTF8.GetString(Convert.FromBase64String(request.UserId));
            var token = Encoding.UTF8.GetString(Convert.FromBase64String(request.Token));
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return ConfirmPasswordCommandResult.Error("UserNotFound");
            }

            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);

            return new ConfirmPasswordCommandResult
            {
                Success = result.Succeeded,
                ErrorCode = result.Errors.Select(e => e.Code).FirstOrDefault()
            };
        }
    }
}
