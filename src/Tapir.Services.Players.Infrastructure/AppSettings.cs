﻿namespace Tapir.Services.Players.Infrastructure
{
    public class AppSettings
    {
        public MongoDbSettings? MongoDb { get; set; }
        public required MessageBusSettings MessageBus { get; set; }
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

    public class MessageBusSettings
    {
        public required string Host { get; set; }
        public required int Port { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string QueueName { get; set; }
    }
}
