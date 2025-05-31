using Tapir.Core.Domain;

namespace Tapir.Core.Persistence
{
    public interface IDomainEventStore
    {
        Task<bool> AddAsync(Guid aggregateId, DomainEvent @event, int expectedVersion);
        Task<bool> AddAsync(Guid aggregateId, IEnumerable<DomainEvent> events, int expectedVersion);
        Task<IReadOnlyList<DomainEvent>> GetByAggregateId(Guid aggregateId);
        Task<IReadOnlyList<DomainEvent>> GetByTimestamp(DateTime from, DateTime to);

        Task<DateTime?> GetLastSynchronizationTime();
        Task SetLastSynchronizationTime(DateTime? time);
        Task<bool> SetLastSynchronizationTime(DateTime? time, DateTime? expectedLastSynchronizationTime);
    }
}
