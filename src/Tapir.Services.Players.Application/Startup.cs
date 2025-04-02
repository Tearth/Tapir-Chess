using Microsoft.Extensions.Hosting;
using Tapir.Core.Scheduler;
using Tapir.Services.Players.Application.Tasks;

namespace Tapir.Services.Players.Application
{
    public class Startup : IHostedService
    {
        private readonly ITaskScheduler _taskScheduler;

        public Startup(ITaskScheduler taskScheduler)
        {
            _taskScheduler = taskScheduler;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _taskScheduler.Register(new SynchronizeDomainEventsTask(), "0/1 * * * * ?");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
