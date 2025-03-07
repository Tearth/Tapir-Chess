
using Tapir.Core.Domain;

namespace Tapir.Core.Interfaces
{
    public interface IDomainEventRegistry
    {
        void Add<TEvent>() where TEvent : DomainEvent;
        Type GetAssemblyType(Guid streamGuid, string type);
    }
}
