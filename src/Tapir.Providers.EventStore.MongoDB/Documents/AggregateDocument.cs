namespace Tapir.Providers.EventStore.MongoDB.Documents
{
    public class AggregateDocument
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
}
