using Tapir.Core.Events;
using Tapir.Core.Scheduler;

namespace Tapir.Services.News.Application.Tasks
{
    public class SynchronizeDomainEventsTask : ITask
    {
        private readonly IDomainEventSynchronizer _synchronizer;

        public SynchronizeDomainEventsTask()
        {

        }

        public SynchronizeDomainEventsTask(IDomainEventSynchronizer synchronizer)
        {
            _synchronizer = synchronizer;
        }

        public async Task Run()
        {
            await _synchronizer.PublishUncommittedEvents();
        }
    }
}
