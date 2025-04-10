using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tapir.Core.Messaging;

namespace Tapir.Services.Players.Infrastructure
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
                await scope.ServiceProvider.GetRequiredService<IMessageBus>().Listen();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
