using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tapir.Identity.Application.Auth.Commands.Login;
using Tapir.Identity.Application.Services;
using Tapir.Identity.Infrastructure.Models;
using Tapir.Identity.Infrastructure.Persistence;

namespace Tapir.Identity.Application.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenCommandResult>
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

        public async Task<RefreshTokenCommandResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var userToken = await _databaseContext.RefreshTokens.FirstOrDefaultAsync(p => p.Value == request.RefreshToken);
            if (userToken?.Value == request.RefreshToken)
            {
                var user = await _userManager.FindByIdAsync(userToken.UserId.ToString());
                var roles = await _userManager.GetRolesAsync(user);
                var accessToken = _tokenGenerator.GenerateAccessToken(user.Id, user.UserName, user.Email, roles.ToList());
                var refreshToken = _tokenGenerator.GenerateRefreshToken();

                _databaseContext.RefreshTokens.Remove(userToken);
                await _databaseContext.RefreshTokens.AddAsync(new RefreshToken<Guid>
                {
                    UserId = user.Id,
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
