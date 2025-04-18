﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Tapir.Identity.Infrastructure.Models;
using Tapir.Identity.Infrastructure.Persistence;
using Tapir.Providers.Mailing.MailKit;
using Tapir.Providers.MessageBus.RabbitMQ;
using Tapir.Providers.Scheduler.Quartz;

namespace Tapir.Identity.Infrastructure
{
    public static class Module
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.Get<AppSettings>() ?? throw new InvalidOperationException("AppSettings not found.");

            // Initialization
            services.AddHostedService<Startup>();
            services.AddSingleton(settings);

            // Identity providers
            services
                .AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<DatabaseContext>();

            // Identity options
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = true;
            });

            // Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    if (settings.Jwt == null)
                    {
                        throw new InvalidOperationException("JWT settings not found.");
                    }

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = settings.Jwt.Issuer,
                        ValidAudience = settings.Jwt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Jwt.Secret))
                    };
                });

            // Database
            services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );

            // Mailing
            services.AddMailKitMailing(options =>
            {
                options.Host = settings.Mailing.Host;
                options.Port = settings.Mailing.Port;
                options.UseSsl = settings.Mailing.UseSsl;
                options.Username = settings.Mailing.Username;
                options.Password = settings.Mailing.Password;
                options.From = settings.Mailing.From;
            });

            // Task scheduler
            services.AddQuartzScheduler(cfg =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string not found.");
                }

                cfg.ConnectionString = connectionString;
            });

            // Message bus
            services.AddRabbitMqMessageBus(cfg =>
            {
                cfg.Host = settings.MessageBus.Host;
                cfg.Port = settings.MessageBus.Port;
                cfg.Username = settings.MessageBus.Username;
                cfg.Password = settings.MessageBus.Password;
                cfg.QueueName = settings.MessageBus.QueueName;
            });

            // Logger
            services.AddSerilog((context, cfg) =>
            {
                cfg.ReadFrom.Configuration(configuration);
            });

            return services;
        }
    }
}
