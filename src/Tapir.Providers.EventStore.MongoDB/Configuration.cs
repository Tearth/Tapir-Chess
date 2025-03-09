namespace Tapir.Providers.EventStore.MongoDB
{
    public class Configuration
    {
        public List<ServerConfiguration>? Servers { get; set; }
        public string? DatabaseName { get; set; }

        public string? AuthenticationMethod { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class ServerConfiguration
    {
        public string? Host { get; set; }
        public int? Port { get; set; }
    }
}
