using Microsoft.Extensions.DependencyInjection;
using Tapir.Services.News.Domain;

namespace Tapir.Services.News.Application
{
    public static class Module
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Module).Assembly));
            services.AddDomain();

            return services;
        }
    }
}
