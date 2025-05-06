using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tapir.Core.Messaging;
using Tapir.Core.Scheduler;

namespace Tapir.Services.Games.Application
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
                var taskScheduler = scope.ServiceProvider.GetRequiredService<ITaskScheduler>();
                
                // Tasks
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
