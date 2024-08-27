using MediatR;
using System;

namespace Shared.Messages.Events
{
    public class StockUpdated : INotification
    {
        public Guid OrderId { get; set; }
        public string CustomerContact { get; set; }
        public List<string> Products { get; set; }
        public bool IsStockAvailable { get; set; }
    }

}
