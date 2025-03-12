using MediatR;
using Tapir.Core.Domain;
using Tapir.Core.Interfaces;

namespace Tapir.Core.Persistence
{
    public class AggregateRepository<TRoot> : IAggregateRepository<TRoot> where TRoot: AggregateRoot
    {
        private readonly IMediator _mediator;
        private readonly IDomainEventStore _eventStore;

        public AggregateRepository(IMediator mediator, IDomainEventStore eventStore)
        {
            _mediator = mediator;
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

            await _eventStore.AddAsync(events);
            entity.ClearUncommittedEvents();
        }
    }
}
