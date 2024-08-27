using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Domain.Data
{
    public class StockContext : DbContext
    {
        public StockContext(DbContextOptions<StockContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed initial products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Bag",
                    StockQuantity = 2
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Shoes",
                    StockQuantity = 4
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Umbrella",
                    StockQuantity = 0
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Cardigan",
                    StockQuantity = 0
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Lipstick",
                    StockQuantity = 6
                },
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Perfume",
                    StockQuantity = 5
                }
            );
        }
    }
}
