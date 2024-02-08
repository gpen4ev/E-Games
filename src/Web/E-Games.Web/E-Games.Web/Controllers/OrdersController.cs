using E_Games.Common.DTOs;
using E_Games.Data.Data.Models;
using E_Games.Services.E_Games.Services;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService,
            UserManager<ApplicationUser> userManager,
            ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderItemDto orderItemDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("CreateOrderAsync called with invalid model state.");
                return BadRequest(ModelState);
            }

            var userId = Guid.Parse(_userManager.GetUserId(User)!);
            var orderViewModel = await _orderService.CreateOrderAsync(orderItemDto, userId);

            return CreatedAtAction(nameof(CreateOrderAsync), new { orderId = orderViewModel.OrderId }, orderViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrdersAsync([FromQuery] int? orderId)
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
        [Authorize]
        public async Task<IActionResult> UpdateOrderItemAsync([FromBody] UpdateOrderItemDto updateOrderItemDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("UpdateOrderItemAsync called with invalid model state.");
                return BadRequest(ModelState);
            }

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var updatedOrder = await _orderService.UpdateOrderItemAmountAsync(updateOrderItemDto, userId);
            return Ok(updatedOrder);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteOrderItemsAsync([FromQuery] IEnumerable<int> itemIds)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _orderService.DeleteOrderItemsAsync(itemIds, userId);

            return NoContent();
        }

        [HttpPost("buy")]
        [Authorize]
        public async Task<IActionResult> BuyProductsAsync()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _orderService.BuyProductsAsync(userId);

            return NoContent();
        }
    }
}
