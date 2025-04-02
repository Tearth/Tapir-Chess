namespace Tapir.Services.News.Infrastructure
{
    public class AppSettings
    {
        public DatabaseSettings? Database { get; set; }
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
