using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tapir.Identity.Infrastructure.Models;

namespace Tapir.Identity.Application
{
    public class Startup : IHostedService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Startup(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();

                if (userManager == null || roleManager == null)
                {
                    throw new InvalidOperationException("Cannot resolve services to initialize users and roles.");
                }

                if (!await roleManager.RoleExistsAsync("admin"))
                {
                    var result = await roleManager.CreateAsync(new ApplicationRole("admin"));

                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException(string.Join(", ", result.Errors.Select(p => p.Description)));
                    }
                }

                if (!await roleManager.RoleExistsAsync("user"))
                {
                    var result = await roleManager.CreateAsync(new ApplicationRole("user"));

                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException(string.Join(", ", result.Errors.Select(p => p.Description)));
                    }
                }

                if (await userManager.FindByNameAsync("admin") == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = "admin@localhost",
                        EmailConfirmed = true
                    };
                    var result = await userManager.CreateAsync(user, "Admin123!");
                    
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException(string.Join(", ", result.Errors.Select(p => p.Description)));
                    }

                    await userManager.AddToRolesAsync(user, ["admin", "user"]);
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
