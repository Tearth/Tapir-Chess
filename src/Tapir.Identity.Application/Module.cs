﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tapir.Core.Bus;
using Tapir.Identity.Application.Account.Commands;
using Tapir.Identity.Application.Auth.Commands;
using Tapir.Identity.Application.Auth.Mails.EmailChange;
using Tapir.Identity.Application.Auth.Mails.EmailConfirmation;
using Tapir.Identity.Application.Auth.Mails.PasswordReset;
using Tapir.Identity.Application.Services;

namespace Tapir.Identity.Application
{
    public static class Module
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // Initialization
            services.AddHostedService<Startup>();

            // Services
            services.AddSingleton<IEventBus, EventBus>();
            services.AddScoped<TokenGenerator>();

            // Command handlers
            services.AddScoped<IChangeEmailCommandHandler, ChangeEmailCommandHandler>();
            services.AddScoped<IChangePasswordCommandHandler, ChangePasswordCommandHandler>();
            services.AddScoped<IChangeUsernameCommandHandler, ChangeUsernameCommandHandler>();
            services.AddScoped<IConfirmEmailChangeCommandHandler, ConfirmEmailChangeCommandHandler>();
            services.AddScoped<IConfirmEmailCommandHandler, ConfirmEmailCommandHandler>();
            services.AddScoped<IConfirmPasswordCommandHandler, ConfirmPasswordCommandHandler>();
            services.AddScoped<ISignInCommandHandler, SignInCommandHandler>();
            services.AddScoped<IRefreshTokenCommandHandler, RefreshTokenCommandHandler>();
            services.AddScoped<IRegisterCommandHandler, RegisterCommandHandler>();
            services.AddScoped<IResetPasswordCommandHandler, ResetPasswordCommandHandler>();

            // Query handlers
            services.AddScoped<IGetAccountQueryHandler, GetAccountQueryHandler>();

            // Tasks
            services.AddScoped<EmailConfirmationMailTask>();
            services.AddScoped<EmailChangeMailTask>();
            services.AddScoped<PasswordResetMailTask>();

            return services;
        }
    }
}
