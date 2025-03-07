namespace Tapir.Core.Domain
{
    public abstract class DomainEntity
    {
        public Guid Guid { get; set; }

        private List<DomainEvent> _uncommittedEvents = new List<DomainEvent>();

        public DomainEntity()
        {
            Guid = Guid.NewGuid();
        }

        public DomainEntity(Guid guid)
        {
            Guid = guid;
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
