using E_Games.Common.DTOs;
using E_Games.Data.Data.Models;
using E_Games.Services.E_Games.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Games.Web.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(IOrderService orderService, UserManager<ApplicationUser> userManager)
        {
            _orderService = orderService;
            _userManager = userManager;
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderItemDto orderItemDto)
        {
            var userId = Guid.Parse(_userManager.GetUserId(User)!);
            var orderViewModel = await _orderService.CreateOrderAsync(orderItemDto, userId);

            return CreatedAtAction(nameof(CreateOrder), new { orderId = orderViewModel.OrderId }, orderViewModel);
        }

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetOrders([FromQuery] int? orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!orderId.HasValue)
            {
                var orders = await _orderService.GetOrdersByUserIdAsync(Guid.Parse(userId!));
                return Ok(orders);
            }
            else
            {
                var orderDetails = await _orderService.GetOrderByIdAsync(orderId.Value, Guid.Parse(userId!));
                if (orderDetails == null)
                {
                    return NotFound("Order not found");
                }

                return Ok(orderDetails);
            }
        }

        [HttpPut]
        // [Authorize]
        public async Task<IActionResult> UpdateOrderItem([FromBody] UpdateOrderItemDto updateOrderItemDto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var updatedOrder = await _orderService.UpdateOrderItemAmountAsync(updateOrderItemDto, userId);
            return Ok(updatedOrder);
        }
    }
}
