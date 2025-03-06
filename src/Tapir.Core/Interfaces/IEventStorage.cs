namespace Tapir.Core.Interfaces
{
    public interface IEvent<T>
    {
        int Type { get; set; }
        T Data { get; set; }
    }

    public interface IEventStorage
    {
        Task AddAsync<T, D>(T @event) where T: IEvent<D>;
    }
}
