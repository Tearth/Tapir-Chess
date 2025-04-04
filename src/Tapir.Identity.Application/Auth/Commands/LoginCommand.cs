using MediatR;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Tapir.Core.Commands;
using Tapir.Core.Validation;
using Tapir.Identity.Application.Services;
using Tapir.Identity.Infrastructure.Commands;
using Tapir.Identity.Infrastructure.Models;
using Tapir.Identity.Infrastructure.Persistence;

namespace Tapir.Identity.Application.Auth.Commands
{
    public class LogInCommand
    {
        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string Username { get; set; }

        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string Password { get; set; }
    }

    public class LogInCommandResult : CommandResultBase<LogInCommandResult>
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }

    public interface ILogInCommandHandler : ICommandHandler<LogInCommand, LogInCommandResult>
    {

    }

    public class LogInCommandHandler : ILogInCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenGenerator _tokenGenerator;
        private readonly DatabaseContext _databaseContext;

        public LogInCommandHandler(UserManager<ApplicationUser> userManager, TokenGenerator tokenGenerator, DatabaseContext databaseContext)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _databaseContext = databaseContext;
        }

        public async Task<LogInCommandResult> Process(LogInCommand request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                return LogInCommandResult.Error("UserNotFound");
            }

            if (await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var accessToken = _tokenGenerator.GenerateAccessToken(user.Id, user.UserName, user.Email, roles.ToList());
                var refreshToken = _tokenGenerator.GenerateRefreshToken();

                await _databaseContext.RefreshTokens.AddAsync(new RefreshToken<Guid>
                {
                    UserId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    Value = refreshToken
                });
                await _databaseContext.SaveChangesAsync();

                return new LogInCommandResult
                {
                    Success = true,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            else
            {
                return LogInCommandResult.Error("InvalidPassword");
            }
        }
    }
}
