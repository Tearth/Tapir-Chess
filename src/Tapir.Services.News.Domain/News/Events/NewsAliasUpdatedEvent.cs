using Tapir.Core.Domain;

namespace Tapir.Services.News.Domain.News.Events
{
    public class NewsAliasUpdatedEvent : DomainEvent
    {
        public string Alias { get; set; }

        public NewsAliasUpdatedEvent(Guid aggregateId, string alias) : base(aggregateId)
        {
            Alias = alias;
        }
    }
}
