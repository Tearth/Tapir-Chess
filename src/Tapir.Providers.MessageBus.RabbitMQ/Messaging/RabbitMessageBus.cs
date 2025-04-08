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

        public async Task Send<T>(T message)
        {
            using (var channel = await GetChannel())
            {
                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                await channel.BasicPublishAsync("amq.fanout", string.Empty, body);
            }
        }

        public async Task Listen()
        {
            using (var channel = await GetChannel())
            {
                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += (model, ea) =>
                {
                    return Task.CompletedTask;
                };

                await channel.BasicConsumeAsync(_configuration.QueueName, false, consumer);
            }
        }

        private async Task<IChannel> GetChannel()
        {
            if (_connection == null)
            {
                var factory = new ConnectionFactory
                {
                    HostName = _configuration.Host,
                    Port = _configuration.Port,
                    UserName = _configuration.Username,
                    Password = _configuration.Password
                };

                _connection = await factory.CreateConnectionAsync();

                using (var channel = await _connection.CreateChannelAsync())
                {
                    await channel.QueueDeclareAsync(_configuration.QueueName, true, false, false);
                }
            }

            return await _connection.CreateChannelAsync();
        }
    }
}
