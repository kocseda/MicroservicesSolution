using MediatR;
using System;

namespace Shared.Messages.Events
{
    public class OrderPlaced : INotification
    {
        public Guid OrderId { get; set; }
        public string CustomerContact { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderPlaceItem> Items { get; set; }
    }

    public class OrderPlaceItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
