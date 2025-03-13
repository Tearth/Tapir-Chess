using Tapir.Core.Domain;

namespace Tapir.Core.Events
{
    public interface IDomainEventRegistry
    {
        void Add<TEvent>() where TEvent : DomainEvent;
        Type GetAssemblyType(string type);
    }
}
