namespace Tapir.Services.Players.Infrastructure
{
    public class AppSettings
    {
        public MongoDbSettings? MongoDb { get; set; }
    }

    public class MongoDbSettings
    {
        public List<MongoDbServer>? Servers { get; set; }
        public string? DatabaseName { get; set; }
        public string? AuthenticationMethod { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class MongoDbServer
    {
        public string? Host { get; set; }
        public int? Port { get; set; }
    }
}
