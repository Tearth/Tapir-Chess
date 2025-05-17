using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    public class RefreshTokenCommand
    {
        [Required(ErrorMessage = ValidationErrorCodes.EMPTY_FIELD)]
        public required string RefreshToken { get; set; }
    }

    public class RefreshTokenCommandResult : CommandResultBase<RefreshTokenCommandResult>
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }

    public interface IRefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenCommandResult>
    {

    }

    public class RefreshTokenCommandHandler : IRefreshTokenCommandHandler
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenGenerator _tokenGenerator;
        private readonly DatabaseContext _databaseContext;

        public RefreshTokenCommandHandler(UserManager<ApplicationUser> userManager, TokenGenerator tokenGenerator, DatabaseContext databaseContext)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _databaseContext = databaseContext;
        }

        public async Task<RefreshTokenCommandResult> Process(RefreshTokenCommand command, ClaimsPrincipal? user)
        {
            var userToken = await _databaseContext.RefreshTokens.FirstOrDefaultAsync(p => p.Value == command.RefreshToken);

            if (userToken?.Value == command.RefreshToken)
            {
                var applicationUser = await _userManager.FindByIdAsync(userToken.UserId.ToString());

                if (applicationUser == null)
                {
                    return RefreshTokenCommandResult.Error("UserNotFound");
                }

                var roles = await _userManager.GetRolesAsync(applicationUser);
                var accessToken = _tokenGenerator.GenerateAccessToken(applicationUser.Id, applicationUser.UserName, applicationUser.Email, roles.ToList());
                var refreshToken = _tokenGenerator.GenerateRefreshToken();

                _databaseContext.RefreshTokens.Remove(userToken);

                await _databaseContext.RefreshTokens.AddAsync(new RefreshToken<Guid>
                {
                    UserId = applicationUser.Id,
                    CreatedAt = DateTime.UtcNow,
                    Value = refreshToken
                });
                await _databaseContext.SaveChangesAsync();

                return new RefreshTokenCommandResult
                {
                    Success = true,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            else
            {
                return RefreshTokenCommandResult.Error("InvalidRefreshToken");
            }
        }
    }
}
