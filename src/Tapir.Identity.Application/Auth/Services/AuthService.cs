using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Tapir.Identity.Application.Auth.Requests;
using Tapir.Identity.Application.Auth.Responses;
using Tapir.Identity.Infrastructure.Models;
using Tapir.Identity.Infrastructure.Persistence;

namespace Tapir.Identity.Application.Auth.Services
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DatabaseContext _databaseContext;
        private readonly TokenService _tokenService;

        public AuthService(UserManager<ApplicationUser> userManager, DatabaseContext databaseContext, TokenService tokenService)
        {
            _userManager = userManager;
            _databaseContext = databaseContext;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                return new LoginResponse
                {
                    Success = false
                };
            }

            if (await _userManager.CheckPasswordAsync(user, request.Password))
            {
                var accessToken = _tokenService.GenerateAccessToken(user.Id, user.UserName, user.Email);
                var refreshToken = _tokenService.GenerateRefreshToken();

                await _databaseContext.RefreshTokens.AddAsync(new RefreshToken<Guid>
                {
                    UserId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    Value = refreshToken
                });
                await _databaseContext.SaveChangesAsync();

                return new LoginResponse
                {
                    Success = true,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            else
            {
                return new LoginResponse
                {
                    Success = false
                };
            }
        }

        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email
            };
            var result = await _userManager.CreateAsync(user, request.Password);

            return new RegisterResponse
            {
                Success = result.Succeeded,
                Errors = result.Errors.Select(e => e.Description).ToList()
            };
        }

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request)
        {
            var userToken = await _databaseContext.RefreshTokens.FirstOrDefaultAsync(p => p.Value == request.RefreshToken);
            if (userToken?.Value == request.RefreshToken)
            {
                var user = await _userManager.FindByIdAsync(userToken.UserId.ToString());
                var accessToken = _tokenService.GenerateAccessToken(user.Id, user.UserName, user.Email);
                var refreshToken = _tokenService.GenerateRefreshToken();

                _databaseContext.RefreshTokens.Remove(userToken);
                await _databaseContext.RefreshTokens.AddAsync(new RefreshToken<Guid>
                {
                    UserId = user.Id,
                    CreatedAt = DateTime.UtcNow,
                    Value = refreshToken
                });
                await _databaseContext.SaveChangesAsync();

                return new RefreshTokenResponse
                {
                    Success = true,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            else
            {
                return new RefreshTokenResponse
                {
                    Success = false
                };
            }
        }
    }
}
