
namespace Tapir.Core.Domain
{
    public abstract class AggregateRoot : DomainEntity
    {
        protected AggregateRoot()
        {

        }

        protected AggregateRoot(Guid guid) : base(guid)
        {

        }

        public abstract void ApplyEvent(DomainEvent @event);
    }
}
