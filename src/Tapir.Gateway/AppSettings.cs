namespace Tapir.Gateway
{
    public class AppSettings
    {
        public required string AllowedOrigins { get; set; }
        public required JwtSettings Jwt { get; set; }
    }

    public class JwtSettings
    {
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string Secret { get; set; }
        public required int ExpirationTime { get; set; }
    }
}
