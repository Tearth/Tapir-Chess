using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Messaging;
using Tapir.Providers.MessageBus.RabbitMQ.Messaging;

namespace Tapir.Providers.MessageBus.RabbitMQ
{
    public static class Module
    {
        public static IServiceCollection AddRabbitMqMessageBus(this IServiceCollection services, Action<Configuration> userConfiguration)
        {
            var configuration = new Configuration();
            userConfiguration(configuration);

            // Initialization
            services.AddSingleton(configuration);

            // Mailing
            services.AddSingleton<IMessageBus, RabbitMessageBus>();

            return services;
        }
    }
}
