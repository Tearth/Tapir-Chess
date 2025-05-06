namespace Tapir.Services.Games.Infrastructure
{
    public class AppSettings
    {
        public JwtSettings? Jwt { get; set; }
        public DatabaseSettings? Database { get; set; }
    }

    public class JwtSettings
    {
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string Secret { get; set; }
    }

    public class DatabaseSettings
    {
        public List<DatabaseServer>? Servers { get; set; }
        public string? DatabaseName { get; set; }
        public string? AuthenticationMethod { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class DatabaseServer
    {
        public string? Host { get; set; }
        public int? Port { get; set; }
    }
}
