namespace Tapir.Core.Domain
{
    public abstract class DomainEntity
    {
        public Guid Id { get; set; }

        private readonly List<DomainEvent> _uncommittedEvents = [];

        protected DomainEntity()
        {
            Id = Guid.NewGuid();
        }

        protected DomainEntity(Guid id)
        {
            Id = id;
        }

        public IReadOnlyList<DomainEvent> GetUncommittedEvents()
        {
            return _uncommittedEvents;
        }

        public IReadOnlyList<TEvent> GetUncommittedEvents<TEvent>() where TEvent : DomainEvent
        {
            return GetUncommittedEvents().OfType<TEvent>().ToList();
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
