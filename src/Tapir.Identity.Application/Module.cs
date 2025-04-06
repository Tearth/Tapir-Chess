using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Events;
using Tapir.Identity.Application.Account.Commands;
using Tapir.Identity.Application.Auth.Commands;
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

            services.AddTransient<IDomainEventBus, DomainEventBus>();
            services.AddTransient<IChangePasswordCommandHandler, ChangePasswordCommandHandler>();
            services.AddTransient<IConfirmEmailCommandHandler, ConfirmEmailCommandHandler>();
            services.AddTransient<IConfirmPasswordCommandHandler, ConfirmPasswordCommandHandler>();
            services.AddTransient<ILogInCommandHandler, LogInCommandHandler>();
            services.AddTransient<IRefreshTokenCommandHandler, RefreshTokenCommandHandler>();
            services.AddTransient<IRegisterCommandHandler, RegisterCommandHandler>();
            services.AddTransient<IResetPasswordCommandHandler, ResetPasswordCommandHandler>();

            return services;
        }
    }
}
