namespace Tapir.Core.Bus
{
    public interface IEventHandler<TEvent>
    {
        Task Process(TEvent @event);
    }
}
