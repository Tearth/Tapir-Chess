﻿namespace Tapir.Providers.Mailing.MailKit
{
    public class Configuration
    {
        public string? Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? From { get; set; }
    }
}
