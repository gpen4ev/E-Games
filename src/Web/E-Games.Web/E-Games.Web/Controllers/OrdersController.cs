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

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="model">The order model for creation.</param>
        /// <returns>The created order model.</returns>
        /// <response code="201">Returns the newly created order</response>
        /// <response code="400">If the model is not valid</response>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderItemDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("CreateOrderAsync called with invalid model state.");
                return BadRequest(ModelState);
            }

            var userId = Guid.Parse(_userManager.GetUserId(User)!);
            var orderViewModel = await _orderService.CreateOrderAsync(model, userId);

            return CreatedAtAction(nameof(CreateOrderAsync), new { orderId = orderViewModel.OrderId }, orderViewModel);
        }

        /// <summary>
        /// Retrieves information about a specific orders by an availabe or not ID.
        /// </summary>
        /// <param name="orderId">The ID of the product to retrieve.</param>
        /// <returns>The information about the orders.</returns>
        /// <response code="200">Returns detailed orders information</response>
        /// <response code="404">If the product is not found</response>
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

        /// <summary>
        /// Updates an order item.
        /// </summary>
        /// <param name="model">The product model for update.</param>
        /// <returns>The updated product model.</returns>
        /// <response code="200">Returns the updated order item</response>
        /// <response code="400">If the model is not valid</response>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateOrderItemAsync([FromBody] UpdateOrderItemDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("UpdateOrderItemAsync called with invalid model state.");
                return BadRequest(ModelState);
            }

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var updatedOrder = await _orderService.UpdateOrderItemAmountAsync(model, userId);

            return Ok(updatedOrder);
        }

        /// <summary>
        /// Deletes a product by its IDs.
        /// </summary>
        /// <param name="itemIds">The IDs of the order items to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">Order item deleted successfully</response>
        /// <response code="400">If the order item is not found</response>
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteOrderItemsAsync([FromQuery] IEnumerable<int> itemIds)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            await _orderService.DeleteOrderItemsAsync(itemIds, userId);

            return NoContent();
        }

        /// <summary>
        /// Buys products.
        /// </summary>
        /// <returns>No content.</returns>
        /// <response code="204">Products bought successfully</response>
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
