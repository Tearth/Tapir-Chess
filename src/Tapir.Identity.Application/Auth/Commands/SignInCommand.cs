using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Tapir.Core.Commands;
using Tapir.Core.Validation;
using Tapir.Identity.Application.Services;
using Tapir.Identity.Infrastructure.Commands;
using Tapir.Identity.Infrastructure.Models;
using Tapir.Identity.Infrastructure.Persistence;

namespace Tapir.Identity.Application.Auth.Commands
{
    public class SignInCommand
    {
        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string Username { get; set; }

        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string Password { get; set; }

        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required bool RememberMe { get; set; }
    }

    public class SignInCommandResult : CommandResultBase<SignInCommandResult>
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }

    public interface ISignInCommandHandler : ICommandHandler<SignInCommand, SignInCommandResult>
    {

    }

    public class SignInCommandHandler : ISignInCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenGenerator _tokenGenerator;
        private readonly DatabaseContext _databaseContext;

        public SignInCommandHandler(UserManager<ApplicationUser> userManager, TokenGenerator tokenGenerator, DatabaseContext databaseContext)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _databaseContext = databaseContext;
        }

        public async Task<SignInCommandResult> Process(SignInCommand command, ClaimsPrincipal? user)
        {
            var applicationUser = await _userManager.FindByNameAsync(command.Username) ?? await _userManager.FindByEmailAsync(command.Username);

            if (applicationUser == null)
            {
                return SignInCommandResult.Error("InvalidUsernameOrPassword");
            }

            if (await _userManager.CheckPasswordAsync(applicationUser, command.Password))
            {
                var roles = await _userManager.GetRolesAsync(applicationUser);
                var accessToken = _tokenGenerator.GenerateAccessToken(applicationUser.Id, applicationUser.UserName, applicationUser.Email, roles.ToList());
                var refreshToken = _tokenGenerator.GenerateRefreshToken();

                await _databaseContext.RefreshTokens.AddAsync(new RefreshToken<Guid>
                {
                    UserId = applicationUser.Id,
                    CreatedAt = DateTime.UtcNow,
                    Value = refreshToken
                });
                await _databaseContext.SaveChangesAsync();

                return new SignInCommandResult
                {
                    Success = true,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            else
            {
                return SignInCommandResult.Error("InvalidUsernameOrPassword");
            }
        }
    }
}
