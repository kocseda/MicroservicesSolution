using Order.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Services
{
    public interface IOrderService
    {
        Task<Guid> PlaceOrderAsync(OrderDto order);
    }
}
