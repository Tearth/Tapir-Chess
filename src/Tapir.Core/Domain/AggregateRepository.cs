using Tapir.Core.Interfaces;

namespace Tapir.Core.Domain
{
    public class AggregateRepository<T> : IAggregateRepository<T> where T: AggregateRoot
    {
        private IDomainEventStorage _eventStorage;

        public AggregateRepository(IDomainEventStorage eventStorage)
        {
            _eventStorage = eventStorage;
        }

        public async Task<T> Load(Guid id)
        {
            var entity = (T)Activator.CreateInstance(typeof(T), id);
            var events = await _eventStorage.GetByStreamGuid(id);

            foreach (var @event in events)
            {
                entity.ApplyEvent(@event);
            }

            return entity;
        }

        public async Task Save(T entity)
        {
            foreach (var @event in entity.GetUncommittedEvents())
            {
                await _eventStorage.AddAsync(@event);
            }
            entity.ClearUncommittedEvents();
        }
    }
}
