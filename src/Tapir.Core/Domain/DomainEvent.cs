namespace Tapir.Core.Domain
{
    public abstract class DomainEvent
    {
        public Guid Id { get; set; }
        public Guid StreamId { get; set; }
        public DateTime Timestamp { get; set; }

        public DomainEvent(Guid streamId)
        {
            Id = Guid.NewGuid();
            StreamId = streamId;
            Timestamp = DateTime.UtcNow;
        }
    }
}
