using Tapir.Core.Domain;

namespace Tapir.Core.Interfaces
{
    public interface IAggregateRepository<T> where T: AggregateRoot
    {
        Task<T> Load(Guid id);
        Task Save(T @entity);
    }
}
