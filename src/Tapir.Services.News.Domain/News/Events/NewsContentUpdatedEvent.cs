using Tapir.Core.Domain;

namespace Tapir.Services.News.Domain.News.Events
{
    public class NewsContentUpdatedEvent : DomainEvent
    {
        public string Content { get; set; }

        public NewsContentUpdatedEvent(Guid streamGuid, string content) : base(streamGuid)
        {
            Content = content;
        }
    }
}
