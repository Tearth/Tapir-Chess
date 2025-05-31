using Microsoft.Extensions.Logging;
using Tapir.Core.Bus;
using Tapir.Core.Persistence;

namespace Tapir.Core.Events
{
    public class DomainEventSynchronizer : IDomainEventSynchronizer
    {
        private readonly IEventBus _eventBus;
        private readonly IDomainEventStore _eventStore;
        private readonly ILogger<DomainEventSynchronizer> _logger;

        public DomainEventSynchronizer(IEventBus eventBus, IDomainEventStore eventStore, ILogger<DomainEventSynchronizer> logger)
        {
            _eventBus = eventBus;
            _eventStore = eventStore;
            _logger = logger;
        }

        public async Task PublishEvents(bool rebuild = false)
        {
            var from = DateTime.MinValue;
            var to = DateTime.UtcNow.AddSeconds(-1);
            var lastSynchronizationTime = await _eventStore.GetLastSynchronizationTime() ?? DateTime.MinValue;

            if (!rebuild)
            {
                from = lastSynchronizationTime;
            }

            do
            {
                foreach (var @event in await _eventStore.GetByTimestamp(from, to))
                {
                    try
                    {
                        if (rebuild)
                        {
                            @event.MarkAsReplay();
                        }

                        await _eventBus.Send(@event);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Failed to publish event {@event.Id} from aggregate {@event.AggregateId}");
                    }
                }

                from = to;
                to = DateTime.UtcNow.AddSeconds(-1);
            }
            while ((DateTime.UtcNow - from).TotalSeconds > 5);

            if (rebuild)
            {
                await _eventStore.SetLastSynchronizationTime(to);
            }
            else
            {
                if (!await _eventStore.SetLastSynchronizationTime(to, lastSynchronizationTime))
                {
                    _logger.LogError($"Concurrent write detected while setting last synchronization time");
                }
            }
        }
    }
}
