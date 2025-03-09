using Tapir.Core.Domain;

namespace Tapir.Core.Interfaces
{
    public interface IDomainEventStore
    {
        Task AddAsync(DomainEvent @event);
        Task AddAsync(IEnumerable<DomainEvent> events);
        Task<IEnumerable<DomainEvent>> GetByStreamId(Guid streamId);
    }
}
