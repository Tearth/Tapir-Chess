namespace Tapir.Core.Domain
{
    public abstract class DomainEntity
    {
        public Guid Id { get; set; }

        private List<DomainEvent> _uncommittedEvents = new List<DomainEvent>();

        public DomainEntity()
        {
            Id = Guid.NewGuid();
        }

        public DomainEntity(Guid id)
        {
            Id = id;
        }

        public IEnumerable<DomainEvent> GetUncommittedEvents()
        {
            return _uncommittedEvents;
        }

        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        protected void ApplyUncommittedEvent(DomainEvent @event)
        {
            _uncommittedEvents.Add(@event);
        }
    }
}
