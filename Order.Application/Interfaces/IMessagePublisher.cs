using Shared.Messages.Events;

namespace Order.Application.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublishAsync(OrderPlaced message);
    }
}
