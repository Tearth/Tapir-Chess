using Tapir.Core.Interfaces;

namespace Tapir.Core.Domain
{
    public class AggregateRepository<T> where T: AggregateRoot
    {
        private IDomainEventStorage _eventStorage;

        public AggregateRepository(IDomainEventStorage eventStorage)
        {
            _eventStorage = eventStorage;
        }

        public async Task<T> Load(Guid guid)
        {
            var entity = (T)Activator.CreateInstance(typeof(T), guid);
            var events = await _eventStorage.GetByStreamGuid(guid);

            foreach (var @event in events)
            {
                entity.ApplyEvent(@event);
            }

            return entity;
        }
    }
}
