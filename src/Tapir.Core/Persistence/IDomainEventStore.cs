using Tapir.Core.Domain;

namespace Tapir.Core.Persistence
{
    public interface IDomainEventStore
    {
        Task AddAsync(DomainEvent @event);
        Task AddAsync(IEnumerable<DomainEvent> events);
        Task<IEnumerable<DomainEvent>> GetByAggregateId(Guid aggregateId);
        Task<IEnumerable<DomainEvent>> GetByTimestamp(DateTime from, DateTime to);

        Task<DateTime?> GetLastSynchronizationTime();
        Task SetLastSynchronizationTime(DateTime? time);
    }
}
