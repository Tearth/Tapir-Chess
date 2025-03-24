using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Tapir.Core.Scheduler;
using Tapir.Identity.Application.Auth.Mails.EmailConfirmation;
using Tapir.Identity.Application.Auth.Mails.PasswordReset;
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
        private readonly ITaskScheduler _taskScheduler;

        public AuthService(UserManager<ApplicationUser> userManager, DatabaseContext databaseContext, TokenService tokenService, ITaskScheduler taskScheduler)
        {
            _userManager = userManager;
            _databaseContext = databaseContext;
            _tokenService = tokenService;
            _taskScheduler = taskScheduler;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return new LoginResponse
                {
                    Success = false
                };
            }

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
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Email))
            {
                return new RegisterResponse
                {
                    Success = false
                };
            }

            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            
            if (!result.Succeeded)
            {
                return new RegisterResponse
                {
                    Success = result.Succeeded,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
            
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            await _taskScheduler.Run(new EmailConfirmationMailTask
            {
                To = user.Email,
                UserId = user.Id.ToString(),
                Token = token
            });

            return new RegisterResponse
            {
                Success = true
            };
        }

        public async Task<bool> ConfirmEmail(ConfirmEmailRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.Token))
            {
                return false;
            }

            var userId = Encoding.UTF8.GetString(Convert.FromBase64String(request.UserId));
            var token = Encoding.UTF8.GetString(Convert.FromBase64String(request.Token));
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        public async Task<bool> ResetPassword(ResetPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return false;
            }

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _taskScheduler.Run(new PasswordResetMailTask
            {
                To = user.Email,
                UserId = user.Id.ToString(),
                Token = token
            });

            return true;
        }

        public async Task<bool> ConfirmPassword(ConfirmPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.Password))
            {
                return false;
            }

            var userId = Encoding.UTF8.GetString(Convert.FromBase64String(request.UserId));
            var token = Encoding.UTF8.GetString(Convert.FromBase64String(request.Token));
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, token, request.Password);
            return result.Succeeded;
        }

        public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return new RefreshTokenResponse
                {
                    Success = false
                };
            }

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
