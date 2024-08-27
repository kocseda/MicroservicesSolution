using Shared.Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Application.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublishAsync(StockUpdated message);
    }
}
