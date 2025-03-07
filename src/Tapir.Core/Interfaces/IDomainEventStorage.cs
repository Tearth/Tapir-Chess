using Tapir.Core.Domain;

namespace Tapir.Core.Interfaces
{
    public interface IDomainEventStorage
    {
        Task AddAsync<T>(T @event) where T: DomainEvent;
        Task<IEnumerable<DomainEvent>> GetByStreamGuid(Guid streamGuid);
    }
}
