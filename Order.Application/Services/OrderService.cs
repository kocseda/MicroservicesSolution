using MediatR;
using Order.Application.DTOs;
using Order.Application.Interfaces;
using Order.Domain.Entities;
using Order.Domain.Repositories;
using Shared.Messages.Events;


namespace Order.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IMessagePublisher messagePublisher, IOrderRepository orderRepository)
        {
            _messagePublisher = messagePublisher;
            _orderRepository = orderRepository;
        }

        public async Task<Guid> PlaceOrderAsync(OrderDto orderDto)
        {
            var order = new Order.Domain.Entities.Order
            {
                Id = Guid.NewGuid(),
                CustomerContact = orderDto.CustomerEmail,
                Items = orderDto.Products.Select(p => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = p.ProductId,
                    Quantity = p.Quantity
                }).ToList()
            };

            await _orderRepository.AddAsync(order);

            // Publish order placed event
            await _messagePublisher.PublishAsync(new OrderPlaced
            {
                OrderId = order.Id,
                Items = order.Items.Select(i => new OrderPlaceItem { ProductId = i.ProductId, Quantity = i.Quantity }).ToList(),
                CustomerContact = order.CustomerContact
            });

            return order.Id;
        }
    }
}
