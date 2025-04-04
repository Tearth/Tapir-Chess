using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Commands;
using Tapir.Core.Events;
using Tapir.Core.Types;
using Tapir.Services.News.Application.News.Commands;
using Tapir.Services.News.Application.Tasks;
using Tapir.Services.News.Domain;

namespace Tapir.Services.News.Application
{

    public static class Module
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<Startup>();
            services.AddDomain();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Module).Assembly));
            services.AddTransient<IDomainEventSynchronizer, DomainEventSynchronizer>();
            services.AddTransient<SynchronizeDomainEventsTask>();

            services.AddTransient<ICreateNewsCommandHandler, CreateNewsCommandHandler>();
            services.AddTransient<IDeleteNewsCommandHandler, DeleteNewsCommandHandler>();
            services.AddTransient<IUpdateNewsCommandHandler, UpdateNewsCommandHandler>();

            return services;
        }
    }
}
