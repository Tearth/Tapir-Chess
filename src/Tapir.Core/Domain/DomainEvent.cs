namespace Tapir.Core.Domain
{
    public abstract class DomainEvent
    {
        public Guid Id { get; set; }
        public Guid AggregateId { get; set; }
        public DateTime Timestamp { get; set; }

        private bool _replay;

        protected DomainEvent(Guid aggregateId)
        {
            Id = Guid.NewGuid();
            AggregateId = aggregateId;
            Timestamp = DateTime.UtcNow;
        }

        public void MarkAsReplay()
        {
            _replay = true;
        }

        public bool IsReplay()
        {
            return _replay;
        }
    }
}
