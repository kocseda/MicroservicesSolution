using Microsoft.EntityFrameworkCore;
using Stock.Domain.Data;
using Stock.Domain.Entities;
using Stock.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly StockContext _context;
        public ProductRepository(StockContext context)
        {
            _context = context;
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

    }
}