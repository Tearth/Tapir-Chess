using Tapir.Core.Domain;
using Tapir.Core.Persistence.Exceptions;

namespace Tapir.Core.Persistence
{
    public class AggregateRepository<TRoot> : IAggregateRepository<TRoot> where TRoot: AggregateRoot
    {
        private readonly IDomainEventStore _eventStore;

        public AggregateRepository(IDomainEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<TRoot> Load(Guid id)
        {
            var events = await _eventStore.GetByAggregateId(id);

            if (events.Count == 0)
            {
                throw new AggregateNotFoundException($"Aggregate {id} not found.");
            }

            if (Activator.CreateInstance(typeof(TRoot), id) is not TRoot entity)
            {
                throw new InvalidOperationException($"Aggregate root {typeof(TRoot).Name} could not be instantiated.");
            }

            foreach (var @event in events)
            {
                entity.ApplyEvent(@event);
            }

            return entity;
        }

        public async Task Save(TRoot entity)
        {
            var events = entity.GetUncommittedEvents();
            var expectedVersion = entity.Version - events.Count;

            for (var i = 0; i < 10; i++)
            {
                if (!await _eventStore.AddAsync(entity.Id, events, expectedVersion))
                {
                    entity = await Load(entity.Id);

                    foreach (var @event in events)
                    {
                        entity.ApplyEvent(@event);
                    }

                    expectedVersion = entity.Version - events.Count;
                }
                else
                {
                    entity.ClearUncommittedEvents();
                    return;
                }
            }

            throw new AggregateConcurrentWriteException("Aggregate was modified by another process.");
        }
    }
}
