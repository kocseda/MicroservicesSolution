using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.DTOs
{
    public class OrderDto
    {
        public List<ProductDto> Products { get; set; }
        public string CustomerEmail { get; set; }
    }

    public class ProductDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
