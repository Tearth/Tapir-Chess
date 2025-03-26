using MediatR;
using Microsoft.AspNetCore.Identity;
using Tapir.Identity.Application.Services;
using Tapir.Identity.Infrastructure.Models;
using Tapir.Identity.Infrastructure.Persistence;

namespace Tapir.Identity.Application.Auth.Commands.Login
{
    public class LogInCommandHandler : IRequestHandler<LogInCommand, LogInCommandResponse>
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

        public async Task<LogInCommandResponse> Handle(LogInCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                return new LogInCommandResponse
                {
                    Success = false
                };
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

                return new LogInCommandResponse
                {
                    Success = true,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            else
            {
                return new LogInCommandResponse
                {
                    Success = false
                };
            }
        }
    }
}
