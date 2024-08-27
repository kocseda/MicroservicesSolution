using Order.Application.Interfaces;
using RabbitMQ.Client;
using Shared.Messages.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Order.Infrastructure.Messaging
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessagePublisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "order_exchange", type: ExchangeType.Direct);
        }

        public async Task PublishAsync(OrderPlaced orderPlaced)
        {
            var message = JsonSerializer.Serialize(orderPlaced);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(exchange: "order_exchange",
                                  routingKey: "order_placed_routing_key",
                                  basicProperties: properties,
                                  body: body);
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
