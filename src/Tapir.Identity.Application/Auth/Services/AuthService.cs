using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Tapir.Identity.Application.Auth.Requests;
using Tapir.Identity.Application.Auth.Responses;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Application.Auth.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
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
                var accessToken = GenerateAccessToken(user.Id, user.Email);
                var refreshToken = GenerateRefreshToken();

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

        private string GenerateAccessToken(Guid userId, string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                 new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                 new Claim(JwtRegisteredClaimNames.Email, email),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            var token = new JwtSecurityToken(
                _configuration["JWT:Issuer"],
                _configuration["JWT:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }
    }
}
