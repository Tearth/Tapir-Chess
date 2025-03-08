using Tapir.Core.Exceptions;
using Tapir.Core.Interfaces;

namespace Tapir.Core.Domain
{
    public class DomainEventRegistry : IDomainEventRegistry
    {
        private Dictionary<string, Type> _events = new Dictionary<string, Type>();

        public void Add<TEvent>() where TEvent : DomainEvent
        {
            var type = typeof(TEvent);

            if (_events.ContainsKey(type.Name))
            {
                throw new EventAlreadyRegisteredException($"Event {type.Name} is already registered.");
            }

            if (type.BaseType != typeof(DomainEvent))
            {
                throw new EventInvalidException($"Event {type.Name} is not a valid event.");
            }

            _events.Add(type.Name, typeof(TEvent));
        }

        public Type GetAssemblyType(Guid streamId, string type)
        {
            if (!_events.ContainsKey(type))
            {
                throw new EventNotRegisteredException($"Event {type} is not registered.");
            }

            return _events[type];
        }
    }
}
