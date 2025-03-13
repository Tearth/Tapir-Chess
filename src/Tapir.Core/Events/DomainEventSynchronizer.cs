using MediatR;
using Tapir.Core.Persistence;

namespace Tapir.Core.Events
{
    public class DomainEventSynchronizer : IDomainEventSynchronizer
    {
        private readonly IMediator _mediator;
        private readonly IDomainEventStore _eventStore;

        public DomainEventSynchronizer(IMediator mediator, IDomainEventStore eventStore)
        {
            _mediator = mediator;
            _eventStore = eventStore;
        }

        public async Task PublishUncommittedEvents(DateTime? from = null, DateTime? to = null)
        {
            from ??= await _eventStore.GetLastSynchronizationTime() ?? DateTime.MinValue;
            to ??= DateTime.UtcNow.AddSeconds(-1);

            foreach (var @event in await _eventStore.GetByTimestamp(from.Value, to.Value))
            {
                try
                {
                    await _mediator.Publish(@event);
                }
                catch
                {
                    // Log error
                    throw;
                }
            }

            await _eventStore.SetLastSynchronizationTime(to);
        }
    }
}
