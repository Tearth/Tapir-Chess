using MailKit.Net.Smtp;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Mailing;
using Tapir.Providers.Mailing.MailKit.Client;

namespace Tapir.Providers.Mailing.MailKit
{
    public static class Module
    {
        public static IServiceCollection AddMailKitMailing(this IServiceCollection services, Action<Configuration> userConfiguration)
        {
            var configuration = new Configuration();
            userConfiguration(configuration);

            // Initialization
            services.AddSingleton(configuration);

            // Mailing
            services.AddScoped<IMailClient, MailClient>();
            services.AddScoped<ISmtpClient, SmtpClient>();

            return services;
        }
    }
}
