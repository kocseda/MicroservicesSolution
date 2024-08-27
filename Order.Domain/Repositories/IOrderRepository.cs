using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order.Domain.Entities.Order order);
    }
}
