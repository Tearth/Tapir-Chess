using Microsoft.Extensions.DependencyInjection;

namespace Tapir.Core.Events
{
    public class DomainEventBus : IDomainEventBus
    {
        private readonly IServiceProvider _services;

        public DomainEventBus(IServiceProvider services)
        {
            _services = services;
        }

        public async Task Send<TEvent>(TEvent @event)
        {
            foreach (var service in _services.GetServices(typeof(IDomainEventHandler<TEvent>)))
            {
                await ((IDomainEventHandler<TEvent>)service).Process(@event);
            }
        }
    }
}
