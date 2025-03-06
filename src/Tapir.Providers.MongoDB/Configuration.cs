﻿namespace Tapir.Providers.MongoDB
{
    public class Configuration
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string DatabaseName { get; set; }

        public string AuthenticationMethod { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
