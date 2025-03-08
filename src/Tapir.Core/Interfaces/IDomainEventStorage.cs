using Tapir.Core.Domain;

namespace Tapir.Core.Interfaces
{
    public interface IDomainEventStorage
    {
        Task AddAsync(DomainEvent @event);
        Task<IEnumerable<DomainEvent>> GetByStreamGuid(Guid streamId);
    }
}
