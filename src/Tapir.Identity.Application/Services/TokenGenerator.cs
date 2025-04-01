using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Tapir.Identity.Infrastructure;

namespace Tapir.Identity.Application.Services
{
    public class TokenGenerator
    {
        private readonly AppSettings _settings;
        private const int REFRESH_TOKEN_LENGTH = 128;

        public TokenGenerator(AppSettings settings)
        {
            _settings = settings;
        }

        public string GenerateAccessToken(Guid userId, string? username, string? email, IEnumerable<string> roles)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Jwt.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                 new Claim(JwtRegisteredClaimNames.Name, username ?? ""),
                 new Claim(JwtRegisteredClaimNames.Email, email ?? ""),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            var token = new JwtSecurityToken(
                _settings.Jwt.Issuer,
                _settings.Jwt.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_settings.Jwt.ExpirationTime),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[REFRESH_TOKEN_LENGTH];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }
    }
}
