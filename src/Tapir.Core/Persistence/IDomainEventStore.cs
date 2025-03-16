using Tapir.Core.Domain;

namespace Tapir.Core.Persistence
{
    public interface IDomainEventStore
    {
        Task<bool> AddAsync(Guid aggregateId, DomainEvent @event, int expectedVersion);
        Task<bool> AddAsync(Guid aggregateId, List<DomainEvent> events, int expectedVersion);
        Task<IReadOnlyList<DomainEvent>> GetByAggregateId(Guid aggregateId);
        Task<IEnumerable<DomainEvent>> GetByTimestamp(DateTime from, DateTime to);

        Task<DateTime?> GetLastSynchronizationTime();
        Task SetLastSynchronizationTime(DateTime? time);
    }
}
