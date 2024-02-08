using E_Games.Common.DTOs;
using E_Games.Data.Data.Enums;
using E_Games.Data.Data.Models;
using E_Games.Services.E_Games.Services;
using E_Games.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace E_Games.Tests
{
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<ILogger<OrdersController>> _mockLogger;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            var userIdString = Guid.NewGuid().ToString();

            _mockOrderService = new Mock<IOrderService>();

            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                null!, null!, null!, null!, null!, null!, null!, null!);

            _mockUserManager.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userIdString);

            _mockLogger = new Mock<ILogger<OrdersController>>();
            _controller = new OrdersController(_mockOrderService.Object, _mockUserManager.Object, _mockLogger.Object);

            SetupControllerContext();
        }

        private void SetupControllerContext()
        {
            var userId = Guid.NewGuid().ToString();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public async Task CreateOrder_ReturnsCreatedAtActionResult_WithOrderViewModel()
        {
            // Arrange
            var userId = Guid.Parse(_controller.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var orderItemDto = new CreateOrderItemDto { ProductId = 1, Amount = 2 };
            var orderViewModel = new OrderDto { OrderId = 123, CreationDate = DateTime.UtcNow, Status = Data.Data.Enums.OrderStatus.Pending };

            _mockOrderService.Setup(service => service.CreateOrderAsync(It.IsAny<CreateOrderItemDto>(), It.IsAny<Guid>()))
            .ReturnsAsync(orderViewModel);

            // Act
            var result = await _controller.CreateOrderAsync(orderItemDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<OrderDto>(createdAtActionResult.Value);

            Assert.Equal(orderViewModel.OrderId, returnValue.OrderId);
        }

        [Fact]
        public async Task GetOrdersAsync_ReturnsAllOrders_WhenOrderIdIsNotProvided()
        {
            // Arrange
            var orders = new List<OrderDto>
            {
                new OrderDto
                {
                    OrderId = 1,
                    CreationDate = DateTime.UtcNow.AddDays(-10),
                    Status = OrderStatus.Pending,
                    Items = new List<OrderItemDto>
                    {
                        new OrderItemDto { ProductId = 1, Amount = 2, Price = 10.00m },
                        new OrderItemDto { ProductId = 2, Amount = 1, Price = 20.00m }
                    }
                },
                new OrderDto
                {
                    OrderId = 2,
                    CreationDate = DateTime.UtcNow.AddDays(-5),
                    Status = OrderStatus.Delivered,
                    Items = new List<OrderItemDto>
                    {
                        new OrderItemDto { ProductId = 3, Amount = 1, Price = 15.00m }
                    }
                }
            };

            _mockOrderService.Setup(x => x.GetOrdersByUserIdAsync(It.IsAny<Guid>()))
                             .ReturnsAsync(orders);

            // Act
            var result = await _controller.GetOrdersAsync(null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<OrderDto>>(okResult.Value);

            Assert.Equal(orders.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetOrdersAsync_ReturnsOrderDetails_WhenOrderIdIsProvided()
        {
            // Arrange
            var orderId = 1;
            var orderDto = new OrderDto
            {
                OrderId = 1,
                CreationDate = DateTime.UtcNow.AddDays(-10),
                Status = OrderStatus.Pending,
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto { ProductId = 1, Amount  = 2, Price = 10.00m },
                    new OrderItemDto { ProductId = 2, Amount  = 1, Price = 20.00m }
                }
            };

            _mockOrderService.Setup(x => x.GetOrderByIdAsync(orderId, It.IsAny<Guid>()))
                             .ReturnsAsync(orderDto);

            // Act
            var result = await _controller.GetOrdersAsync(orderId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(orderDto, okResult.Value);
        }

        [Fact]
        public async Task GetOrdersAsync_ReturnsNotFound_WhenOrderDoesNotExist()
        {
            // Arrange
            var orderId = 999;

            _mockOrderService.Setup(x => x.GetOrderByIdAsync(orderId, It.IsAny<Guid>()))
                             .ReturnsAsync((OrderDto)null!);

            // Act
            var result = await _controller.GetOrdersAsync(orderId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
