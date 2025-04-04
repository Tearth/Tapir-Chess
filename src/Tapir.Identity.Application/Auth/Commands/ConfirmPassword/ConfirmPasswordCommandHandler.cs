﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Application.Auth.Commands.ConfirmPassword
{
    public class ConfirmPasswordCommandHandler : IRequestHandler<ConfirmPasswordCommand, ConfirmPasswordCommandResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmPasswordCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ConfirmPasswordCommandResult> Handle(ConfirmPasswordCommand request, CancellationToken cancellationToken)
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
