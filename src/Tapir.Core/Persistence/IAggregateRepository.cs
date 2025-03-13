using Tapir.Core.Domain;

namespace Tapir.Core.Persistence
{
    public interface IAggregateRepository<TRoot> where TRoot: AggregateRoot
    {
        Task<TRoot> Load(Guid id);
        Task Save(TRoot @entity);
    }
}
