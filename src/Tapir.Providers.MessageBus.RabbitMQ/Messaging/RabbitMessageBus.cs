using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Tapir.Core.Bus;
using Tapir.Core.Messaging;

namespace Tapir.Providers.MessageBus.RabbitMQ.Messaging
{
    public class RabbitMessageBus : IMessageBus
    {
        private IConnection? _connection;
        private readonly IEventBus _eventBus;
        private readonly Configuration _configuration;

        public RabbitMessageBus(IEventBus eventBus, Configuration configuration)
        {
            _eventBus = eventBus;
            _configuration = configuration;
        }

        public async Task Send<T>(T message) where T : notnull
        {
            using (var channel = await GetChannel())
            {
                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                var header = new BasicProperties
                {
                    Type = message.GetType().AssemblyQualifiedName
                };

                await channel.BasicPublishAsync("amq.fanout", string.Empty, true, header, body);
            }
        }

        public async Task Listen()
        {
            var channel = await GetChannel();
            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, args) =>
            {
                if (string.IsNullOrEmpty(args.BasicProperties.Type))
                {
                    throw new InvalidOperationException("Received message cannot be recognized.");
                }

                var type = Type.GetType(args.BasicProperties.Type);
                
                if (type == null)
                {
                    throw new InvalidOperationException("Message type not found.");
                }

                var json = Encoding.UTF8.GetString(args.Body.ToArray());
                var message = JsonSerializer.Deserialize(json, type);

                if (message == null)
                {
                    throw new InvalidOperationException("Deserialized message is empty");
                }

                await _eventBus.Send(message);
                await channel.BasicAckAsync(args.DeliveryTag, false);
            };

            if (string.IsNullOrEmpty(_configuration.QueueName))
            {
                throw new InvalidOperationException("Empty queue name.");
            }

            await channel.BasicConsumeAsync(_configuration.QueueName, false, consumer);
        }

        private async Task<IChannel> GetChannel()
        {
            if (_connection == null)
            {
                var factory = new ConnectionFactory
                {
                    HostName = _configuration.Host ?? "",
                    Port = _configuration.Port,
                    UserName = _configuration.Username ?? "",
                    Password = _configuration.Password ?? ""
                };

                _connection = await factory.CreateConnectionAsync();

                using (var channel = await _connection.CreateChannelAsync())
                {
                    if (string.IsNullOrEmpty(_configuration.QueueName))
                    {
                        throw new InvalidOperationException("Empty queue name.");
                    }

                    await channel.QueueDeclareAsync(_configuration.QueueName, true, false, false);
                    await channel.QueueBindAsync(_configuration.QueueName, "amq.fanout", string.Empty);
                }
            }

            return await _connection.CreateChannelAsync();
        }
    }
}
