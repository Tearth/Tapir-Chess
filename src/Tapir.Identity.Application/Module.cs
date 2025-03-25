using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Identity.Application.Account.Services;
using Tapir.Identity.Application.Auth.Mails.EmailConfirmation;
using Tapir.Identity.Application.Auth.Mails.PasswordReset;
using Tapir.Identity.Application.Auth.Services;

namespace Tapir.Identity.Application
{
    public static class Module
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<AuthService>();
            services.AddTransient<TokenService>();
            services.AddTransient<AccountService>();
            services.AddTransient<EmailConfirmationMailTask>();
            services.AddTransient<PasswordResetMailTask>();
            services.AddHostedService<Startup>();
            return services;
        }
    }
}
