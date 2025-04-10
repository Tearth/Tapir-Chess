using Microsoft.Extensions.DependencyInjection;

namespace Tapir.Core.Bus
{
    public class EventBus : IEventBus
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EventBus(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Send<TEvent>(TEvent @event) where TEvent: notnull
        {
            var eventType = @event.GetType();
            var handlerType = typeof(IEventHandler<>).MakeGenericType(eventType);

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                foreach (var service in scope.ServiceProvider.GetServices(handlerType))
                {
                    var serviceType = service!.GetType();
                    var method = serviceType.GetMethod("Process");

                    await (Task)method!.Invoke(service, [@event])!;
                }
            }
        }
    }
}
