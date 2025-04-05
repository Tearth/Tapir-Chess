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

        public async Task Send<TEvent>(TEvent @event) where TEvent: notnull
        {
            var eventType = @event.GetType();
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);

            foreach (var service in _services.GetServices(handlerType))
            {
                var serviceType = service!.GetType();
                var method = serviceType.GetMethod("Process");

                await (Task)method!.Invoke(service, [@event])!;
            }
        }
    }
}
