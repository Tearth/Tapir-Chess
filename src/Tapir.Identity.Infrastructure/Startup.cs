using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tapir.Core.Messaging;
using Tapir.Identity.Infrastructure.Persistence;

namespace Tapir.Identity.Infrastructure
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
                await scope.ServiceProvider.GetRequiredService<DatabaseContext>().Database.MigrateAsync();
                await scope.ServiceProvider.GetRequiredService<IMessageBus>().Listen();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
