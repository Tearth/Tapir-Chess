
namespace Tapir.Core.Domain
{
    public abstract class AggregateRoot : DomainEntity
    {
        public int Version { get; private set; }

        public virtual void ApplyEvent(DomainEvent @event)
        {
            Version += 1;
        }
    }
}
