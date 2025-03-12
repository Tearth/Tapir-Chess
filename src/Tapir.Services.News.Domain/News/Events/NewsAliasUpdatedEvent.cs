using MediatR;
using Tapir.Core.Domain;

namespace Tapir.Services.News.Domain.News.Events
{
    public class NewsAliasUpdatedEvent : DomainEvent, INotification
    {
        public string Alias { get; set; }

        public NewsAliasUpdatedEvent(Guid aggregateId, string alias) : base(aggregateId)
        {
            Alias = alias;
        }
    }
}
