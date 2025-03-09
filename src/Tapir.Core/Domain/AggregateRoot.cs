
namespace Tapir.Core.Domain
{
    public abstract class AggregateRoot : DomainEntity
    {
        public int Version { get; private set; }

        protected AggregateRoot()
        {

        }

        protected AggregateRoot(Guid id) : base(id)
        {

        }

        public virtual void ApplyEvent(DomainEvent @event)
        {
            Version += 1;
        }
    }
}
