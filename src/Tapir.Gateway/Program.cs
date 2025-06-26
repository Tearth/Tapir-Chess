using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OpenTelemetry.Metrics;
using Serilog;
using Yarp.ReverseProxy.Transforms;

namespace Tapir.Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var settings = builder.Configuration.Get<AppSettings>();

            if (settings == null)
            {
                throw new InvalidOperationException("AppSettings not found.");
            }

            builder.Services.AddControllers();
            builder.Services
                .AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
                .AddTransforms(transformBuilderContext =>
                {
                    transformBuilderContext.AddRequestTransform(transformContext =>
                    {
                        var accessToken = transformContext.HttpContext.Request.Cookies["access_token"];
                        var refreshToken = transformContext.HttpContext.Request.Cookies["refresh_token"];
                        var rememberMe = transformContext.HttpContext.Request.Cookies["remember_me"];

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            transformContext.ProxyRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                        }

                        if (!string.IsNullOrEmpty(refreshToken))
                        {
                            transformContext.ProxyRequest.Headers.Add("X-Refresh-Token", refreshToken);
                        }

                        if (!string.IsNullOrEmpty(rememberMe))
                        {
                            transformContext.ProxyRequest.Headers.Add("X-Remember-Me", rememberMe ?? "false");
                        }

                        return ValueTask.CompletedTask;
                    });
                });

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
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
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.HttpContext.Request.Cookies["access_token"];

                            if (!string.IsNullOrEmpty(accessToken))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireUser", policy =>
                {
                    policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "user");
                });

                options.AddPolicy("RequireAdmin", policy =>
                {
                    policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "admin");
                });
            });

            builder.Services.AddSerilog((context, cfg) =>
            {
                cfg.ReadFrom.Configuration(builder.Configuration);
            });

            // OpenTelemetry
            builder.Services.AddOpenTelemetry().WithMetrics(cfg =>
            {
                cfg.AddPrometheusExporter();
                cfg.AddMeter("Microsoft.AspNetCore.Hosting", "Microsoft.AspNetCore.Server.Kestrel");
                cfg.AddView("http.server.request.duration", new ExplicitBucketHistogramConfiguration
                {
                    Boundaries = [0, 0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10]
                });
            });

            var app = builder.Build();
            app.UseCors(x => x
                .WithOrigins(settings.AllowedOrigins)
                .WithExposedHeaders("Location")
                .AllowAnyMethod()
                .AllowCredentials()
                .AllowAnyHeader());
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.MapReverseProxy();
            app.Run();
        }
    }
}
