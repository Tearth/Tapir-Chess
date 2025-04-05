using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Events;
using Tapir.Services.Players.Application.Tasks;
using Tapir.Services.Players.Domain;

namespace Tapir.Services.Players.Application
{

    public static class Module
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<Startup>();
            services.AddDomain();
            services.AddTransient<IDomainEventSynchronizer, DomainEventSynchronizer>();
            services.AddTransient<SynchronizeDomainEventsTask>();

            return services;
        }
    }
}
