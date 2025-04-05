using Tapir.Core.Domain;

namespace Tapir.Services.News.Domain.News.Events
{
    public class NewsCreatedEvent : DomainEvent
    {
        public DateTime CreatedAt { get; set; }

        public NewsCreatedEvent(Guid aggregateId, DateTime createdAt) : base(aggregateId)
        {
            CreatedAt = createdAt;
        }
    }
}
