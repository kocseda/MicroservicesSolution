using Stock.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stock.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(Guid id);
        Task UpdateAsync(Product product);
    }
}