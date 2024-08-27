using Microsoft.AspNetCore.Mvc;
using Order.Application.DTOs;
using Order.Application.Services;
using Order.Domain.Entities;

namespace Order.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderDto order)
        {
            var orderId = await _orderService.PlaceOrderAsync(order);
            return Ok(orderId);
        }
    } 
}