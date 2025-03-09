using MediatR;
using Tapir.Core.Domain;

namespace Tapir.Services.News.Domain.News.Events
{
    public class NewsAliasUpdatedEvent : DomainEvent, INotification
    {
        public string Alias { get; set; }

        public NewsAliasUpdatedEvent(Guid streamId, string alias) : base(streamId)
        {
            Alias = alias;
        }
    }
}
