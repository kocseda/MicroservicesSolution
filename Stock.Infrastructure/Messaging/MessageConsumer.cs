using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Messages.Events;
using Stock.Application.Handlers;
using Stock.Application.Interfaces;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Stock.Infrastructure.Messaging
{
    public class MessageConsumer : IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public MessageConsumer(IServiceProvider serviceProvider)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _serviceProvider = serviceProvider;

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "order_exchange", type: ExchangeType.Direct);
            _channel.QueueDeclare(queue: "stock_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: "stock_queue", exchange: "order_exchange", routingKey: "order_placed_routing_key");
        }

        public void StartConsuming()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var orderPlaced = JsonSerializer.Deserialize<OrderPlaced>(message);

                    if (orderPlaced != null)
                    {
                        // Send the message to MediatR for handling
                        await mediator.Publish(new OrderPlacedNotification { OrderPlaced = orderPlaced });
                    }
                }
            };

            _channel.BasicConsume(queue: "stock_queue", autoAck: true, consumer: consumer);
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }

    }
}