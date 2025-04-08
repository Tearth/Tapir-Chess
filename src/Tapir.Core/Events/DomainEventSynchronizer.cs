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

        public async Task PublishUncommittedEvents(DateTime? from = null, DateTime? to = null)
        {
            from ??= await _eventStore.GetLastSynchronizationTime() ?? DateTime.MinValue;
            to ??= DateTime.UtcNow.AddSeconds(-1);

            foreach (var @event in await _eventStore.GetByTimestamp(from.Value, to.Value))
            {
                try
                {
                    await _eventBus.Send(@event);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to publish event {@event.Id} from aggregate {@event.AggregateId}");
                }
            }

            await _eventStore.SetLastSynchronizationTime(to);
        }
    }
}
