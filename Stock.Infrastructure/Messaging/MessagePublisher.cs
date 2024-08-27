using RabbitMQ.Client;
using Shared.Messages.Events;
using Stock.Application.Interfaces;
using System.Text;
using System.Text.Json;

namespace Stock.Infrastructure.Messaging
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

            _channel.ExchangeDeclare(exchange: "stock_exchange", type: ExchangeType.Direct);
        }

        public async Task PublishAsync(StockUpdated stockUpdated)
        {
            var message = JsonSerializer.Serialize(stockUpdated);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _channel.BasicPublish(exchange: "stock_exchange",
                                  routingKey: "stock_updated_routing_key",
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
