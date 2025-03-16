using MediatR;
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
            if (Activator.CreateInstance(typeof(TRoot), id) is not TRoot entity)
            {
                throw new InvalidOperationException($"Aggregate root {typeof(TRoot).Name} could not be instantiated.");
            }

            foreach (var @event in await _eventStore.GetByAggregateId(id))
            {
                entity.ApplyEvent(@event);
            }

            return entity;
        }

        public async Task Save(TRoot entity)
        {
            var events = entity.GetUncommittedEvents();
            var expectedVersion = entity.Version - events.Count;

            if (!await _eventStore.AddAsync(entity.Id, events.ToList(), expectedVersion))
            {
                throw new AggregateConcurrentWriteException("Aggregate was modified by another process.");
            }

            entity.ClearUncommittedEvents();
        }
    }
}
