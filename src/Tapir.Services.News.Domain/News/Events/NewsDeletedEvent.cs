using Tapir.Core.Domain;

namespace Tapir.Services.News.Domain.News.Events
{
    public class NewsDeletedEvent : DomainEvent
    {
        public DateTime DeletedAt { get; set; }

        public NewsDeletedEvent(Guid aggregateId, DateTime deletedAt) : base(aggregateId)
        {
            DeletedAt = deletedAt;
        }
    }
}
