using Tapir.Core.Domain;

namespace Tapir.Services.News.Domain.News.Events
{
    public class NewsTitleUpdatedEvent : DomainEvent
    {
        public string Title { get; set; }

        public NewsTitleUpdatedEvent(Guid streamId, string title) : base(streamId)
        {
            Title = title;
        }
    }
}
