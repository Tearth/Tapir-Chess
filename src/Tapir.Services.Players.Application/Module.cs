using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Bus;
using Tapir.Core.Events;
using Tapir.Core.Messaging.Identity;
using Tapir.Services.Players.Application.Messages;
using Tapir.Services.Players.Application.Tasks;
using Tapir.Services.Players.Domain;

namespace Tapir.Services.Players.Application
{

    public static class Module
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // Initialization
            services.AddHostedService<Startup>();
            services.AddDomain();

            // Services
            services.AddScoped<IDomainEventSynchronizer, DomainEventSynchronizer>();

            // Tasks
            services.AddScoped<SynchronizeDomainEventsTask>();
            
            // Message handlers
            services.AddScoped<IEventHandler<UserCreatedMessage>, UserCreatedMessageHandler>();

            return services;
        }
    }
}
