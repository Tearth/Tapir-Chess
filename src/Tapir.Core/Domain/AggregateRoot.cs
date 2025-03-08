
namespace Tapir.Core.Domain
{
    public abstract class AggregateRoot : DomainEntity
    {
        protected AggregateRoot()
        {

        }

        protected AggregateRoot(Guid id) : base(id)
        {

        }

        public abstract void ApplyEvent(DomainEvent @event);
    }
}
