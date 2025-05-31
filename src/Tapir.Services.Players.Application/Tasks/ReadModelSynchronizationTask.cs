using Microsoft.Extensions.Logging;
using Tapir.Core.Events;
using Tapir.Core.Scheduler;

namespace Tapir.Services.Players.Application.Tasks
{
    public class ReadModelSynchronizationTask : ITask
    {
        private readonly IDomainEventSynchronizer _synchronizer;

        public ReadModelSynchronizationTask(IDomainEventSynchronizer? synchronizer = null)
        {
            _synchronizer = synchronizer!;
        }

        public async Task Run()
        {
            await _synchronizer.PublishEvents();
        }
    }
}
