namespace Tapir.Core.Events
{
    public interface IDomainEventSynchronizer
    {
        Task PublishEvents(bool rebuild = false);
    }
}
