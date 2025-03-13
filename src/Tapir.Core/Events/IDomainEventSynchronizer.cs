namespace Tapir.Core.Events
{
    public interface IDomainEventSynchronizer
    {
        Task PublishUncommittedEvents(DateTime? from = null, DateTime? to = null);
    }
}
