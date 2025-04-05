using Tapir.Core.Domain;

namespace Tapir.Services.News.Domain.News.Events
{
    public class NewsContentUpdatedEvent : DomainEvent
    {
        public string Content { get; set; }

        public NewsContentUpdatedEvent(Guid aggregateId, string content) : base(aggregateId)
        {
            Content = content;
        }
    }
}
