﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Events;
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

            return services;
        }
    }
}
