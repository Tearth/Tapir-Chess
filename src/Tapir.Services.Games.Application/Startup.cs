using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tapir.Core.Scheduler;
using Tapir.Services.Games.Application.Tasks;

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
                await taskScheduler.Register(new DomainEventSynchronizationTask(), "0/1 * * * * ?");
                await taskScheduler.Register(new MatchmakingTask(), "0/1 * * * * ?");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
