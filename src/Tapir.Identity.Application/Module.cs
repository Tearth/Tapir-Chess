using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Identity.Application.Auth.Mails.EmailConfirmation;
using Tapir.Identity.Application.Auth.Mails.PasswordReset;
using Tapir.Identity.Application.Services;

namespace Tapir.Identity.Application
{
    public static class Module
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<Startup>();
            services.AddTransient<TokenGenerator>();
            services.AddTransient<EmailConfirmationMailTask>();
            services.AddTransient<PasswordResetMailTask>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Module).Assembly));
            return services;
        }
    }
}
