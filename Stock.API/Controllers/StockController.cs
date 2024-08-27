using Microsoft.AspNetCore.Mvc;
using Stock.Domain.Repositories;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public StockController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductStock(Guid productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null) return NotFound();

            return Ok(product.StockQuantity);
        }
    }
}
