namespace Tapir.Core.Domain
{
    public abstract class DomainEvent
    {
        public Guid Guid { get; set; }
        public string Type { get; set; }
        public Guid StreamGuid { get; set; }
        public DateTime Timestamp { get; set; }

        public DomainEvent(Guid streamGuid)
        {
            Guid = Guid.NewGuid();
            StreamGuid = streamGuid;
            Timestamp = DateTime.UtcNow;
            Type = GetType().Name;
        }
    }
}
