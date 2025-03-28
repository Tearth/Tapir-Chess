﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tapir.Identity.Infrastructure.Models;
using Tapir.Identity.Infrastructure.Persistence;
using Tapir.Providers.Mailing.MailKit;
using Tapir.Providers.Scheduler.Quartz;

namespace Tapir.Identity.Infrastructure
{
    public static class Module
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.Get<AppSettings>();
            if (settings == null)
            {
                throw new InvalidOperationException("AppSettings not found.");
            }

            services.AddDbContext<DatabaseContext>(options => 
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );

            services
                .AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<DatabaseContext>();

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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    if (settings.JWT == null)
                    {
                        throw new InvalidOperationException("JWT settings not found.");
                    }

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = settings.JWT.Issuer,
                        ValidAudience = settings.JWT.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.JWT.Secret))
                    };
                });

            services.AddMailKitMailing(options =>
            {
                options.Host = configuration["Mailing:Host"];
                options.Port = int.Parse(configuration["Mailing:Port"]);
                options.UseSsl = bool.Parse(configuration["Mailing:UseSsl"]);
                options.Username = configuration["Mailing:Username"];
                options.Password = configuration["Mailing:Password"];
                options.From = configuration["Mailing:From"];
            });

            services.AddQuartzScheduler(cfg =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string not found.");
                }

                cfg.ConnectionString = connectionString;
            });

            services.AddHostedService<Startup>();

            return services;
        }
    }
}
