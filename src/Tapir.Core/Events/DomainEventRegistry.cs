using Tapir.Core.Domain;

namespace Tapir.Core.Events
{
    public class DomainEventRegistry : IDomainEventRegistry
    {
        private readonly Dictionary<string, Type> _events = [];

        public void Add<TEvent>() where TEvent : DomainEvent
        {
            var type = typeof(TEvent);

            if (_events.ContainsKey(type.Name))
            {
                throw new InvalidOperationException($"Event {type.Name} is already registered.");
            }

            if (type.BaseType != typeof(DomainEvent))
            {
                throw new InvalidOperationException($"Event {type.Name} is not a valid type.");
            }

            _events.Add(type.Name, type);
        }

        public Type GetAssemblyType(string type)
        {
            if (!_events.TryGetValue(type, out var value))
            {
                throw new InvalidOperationException($"Event {type} is not registered.");
            }

            return value;
        }
    }
}
