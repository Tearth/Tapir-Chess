using Tapir.Core.Domain;

namespace Tapir.Services.News.Domain.News.Events
{
    public class NewsAliasUpdatedEvent : DomainEvent
    {
        public string Alias { get; set; }

        public NewsAliasUpdatedEvent(Guid streamGuid, string alias) : base(streamGuid)
        {
            Alias = alias;
        }
    }
}
