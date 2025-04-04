﻿namespace Tapir.Identity.Infrastructure
{
    public class AppSettings
    {
        public required JwtSettings Jwt { get; set; }
        public required MailingSettings Mailing { get; set; }
        public required EndpointsSettings Endpoints { get; set; }
    }

    public class JwtSettings
    {
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string Secret { get; set; }
        public required int ExpirationTime { get; set; }
    }

    public class MailingSettings
    {
        public required string Host { get; set; }
        public required int Port { get; set; }
        public required bool UseSsl { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string From { get; set; }
    }

    public class EndpointsSettings
    {
        public required string EmailConfirmation { get; set; }
        public required string PasswordReset { get; set; }
    }
}
