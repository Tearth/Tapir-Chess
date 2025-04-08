namespace Tapir.Core.Bus
{
    public interface IEventBus
    {
        Task Send<TEvent>(TEvent @event) where TEvent : notnull;
    }
}
