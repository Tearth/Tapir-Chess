using Tapir.Core.Events;
using Tapir.Core.Scheduler;

namespace Tapir.Services.Games.Application.Tasks
{
    public class DomainEventSynchronizationTask : ITask
    {
        private readonly IDomainEventSynchronizer _synchronizer;

        public DomainEventSynchronizationTask(IDomainEventSynchronizer? synchronizer = null)
        {
            _synchronizer = synchronizer!;
        }

        public async Task Run()
        {
            await _synchronizer.PublishUncommittedEvents();
        }
    }
}
