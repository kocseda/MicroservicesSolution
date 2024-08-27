using MediatR;
using Stock.Domain.Repositories;
using Stock.Application.Interfaces;
using Shared.Messages.Events;
using Stock.Application.Services;

namespace Stock.Application.Handlers
{
    public class OrderPlacedNotification : INotification
    {
        public OrderPlaced OrderPlaced { get; set; }
    }

    public class OrderPlacedHandler : INotificationHandler<OrderPlacedNotification>
    {
        private readonly IProductService _productService;
        private readonly IMessagePublisher _messagePublisher;

        public OrderPlacedHandler(IProductService productService, IMessagePublisher messagePublisher)
        {
            _productService = productService;
            _messagePublisher = messagePublisher;
        }

        public async Task Handle(OrderPlacedNotification notification, CancellationToken cancellationToken)
        {
            bool allItemsInStock = true;
            var products = new List<string>();
            var orderPlaced = notification.OrderPlaced;

            foreach (var item in orderPlaced.Items)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductId);
                if (product.StockQuantity < item.Quantity)
                {
                    products.Add(product.Name);
                    allItemsInStock = false;
                    break;
                }
            }

            if (allItemsInStock)
            {
                string productName = string.Empty;
                // Deduct stock
                foreach (var item in orderPlaced.Items)
                {
                    var product = await _productService.GetProductByIdAsync(item.ProductId);
                    product.StockQuantity -= item.Quantity;
                    productName = product.Name;
                    await _productService.UpdateProductStockAsync(product);
                }

                await _messagePublisher.PublishAsync(new StockUpdated { OrderId = orderPlaced.OrderId,
                    CustomerContact = orderPlaced.CustomerContact, IsStockAvailable = allItemsInStock });
            }
            else
            {
                await _messagePublisher.PublishAsync(new StockUpdated { OrderId = orderPlaced.OrderId,
                    CustomerContact = orderPlaced.CustomerContact, Products = products, IsStockAvailable = allItemsInStock });
            }
        }
    }
}
