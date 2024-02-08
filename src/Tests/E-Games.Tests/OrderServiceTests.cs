using AutoMapper;
using E_Games.Common;
using E_Games.Common.DTOs;
using E_Games.Data.Data;
using E_Games.Data.Data.Enums;
using E_Games.Data.Data.Models;
using E_Games.Services.E_Games.Services;
using E_Games.Web.Mapping;
using Microsoft.EntityFrameworkCore;

namespace E_Games.Tests
{
    public class OrderServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly OrderService _orderService;
        private readonly IMapper _mapper;
        private readonly Guid _userId;

        public OrderServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            _context = new ApplicationDbContext(options);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });

            _mapper = mockMapper.CreateMapper();

            _userId = Guid.NewGuid();

            SeedDatabase();

            _orderService = new OrderService(_context, _mapper);
        }

        private void SeedDatabase()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var user = new ApplicationUser { Id = _userId };

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "The Witcher 3: Wild Hunt", Platform = Platforms.PC },
                new Product { Id = 2, Name = "Red Dead Redemption 2", Platform = Platforms.Nintendo },
                new Product { Id = 3, Name = "Grand Theft Auto V", Platform = Platforms.Xbox },
                new Product { Id = 4, Name = "The Legend of Zelda", Platform = Platforms.Xbox },
                new Product { Id = 5, Name = "Dark Souls III", Platform = Platforms.PC },
                new Product { Id = 6, Name = "The Elder Scrolls V: Skyrim", Platform = Platforms.Nintendo },
                new Product { Id = 7, Name = "Sekiro: Shadows Die Twice", Platform = Platforms.Xbox },
                new Product { Id = 8, Name = "Bloodborne", Platform = Platforms.PC },
                new Product { Id = 9, Name = "Mass Effect 2", Platform = Platforms.Xbox },
                new Product { Id = 10, Name = "Hollow Knight", Platform = Platforms.Mobile },
                new Product { Id = 11, Name = "The Last of Us Part II", Platform = Platforms.PC },
                new Product { Id = 51, Name = "Fifa 2026", Platform = Platforms.PC },
            };

            var order = new Order
            {
                OrderId = 1,
                UserId = user.Id,
                OrderItems = new List<OrderItem>
                {
                new OrderItem { OrderItemId = 1, Product = products[0], Quantity = 2 },
                new OrderItem { OrderItemId = 2, Product = products[1], Quantity = 2 },
                new OrderItem { OrderItemId = 3, Product = products[2], Quantity = 2 },
                new OrderItem { OrderItemId = 4, Product = products[3], Quantity = 2 },
                new OrderItem { OrderItemId = 5, Product = products[11], Quantity = 0 },
                },
                Status = OrderStatus.Pending
            };

            _context.Users.Add(user);
            _context.Products.AddRange(products);
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        [Fact]
        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task GetOrderByIdAsync_ReturnsOrder_WhenOrderExists()
        {
            // Arrange
            var orderId = 1;
            var userId = _userId;

            // Act
            var result = await _orderService.GetOrderByIdAsync(orderId, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.OrderId);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ReturnsNull_WhenOrderDoesNotExist()
        {
            // Arrange
            var nonExistingOrderId = 99;
            var userId = _userId;

            // Act
            var result = await _orderService.GetOrderByIdAsync(nonExistingOrderId, userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetOrdersByUserIdAsync_ReturnsAllOrdersForUser()
        {
            // Arrange
            var userId = _userId;

            // Act
            var result = await _orderService.GetOrdersByUserIdAsync(userId);

            // Assert
            var orderList = result.ToList();
            Assert.NotEmpty(orderList);
        }

        [Fact]
        public async Task DeleteOrderItemsAsync_DeletesItems_WhenItemsExistForUser()
        {
            // Arrange
            var userId = _userId;
            var itemIds = new List<int> { 1, 2 };

            // Act
            await _orderService.DeleteOrderItemsAsync(itemIds, userId);

            // Assert
            var remainingItems = await _context.OrderItems
                .Where(oi => itemIds.Contains(oi.OrderItemId)).ToListAsync();

            Assert.Empty(remainingItems);
        }

        [Fact]
        public async Task DeleteOrderItemsAsync_ThrowsError_WhenNoItemsMatch()
        {
            // Arrange
            var userId = _userId;
            var itemIds = new List<int> { 100, 101 };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiExceptionBase>(()
                => _orderService.DeleteOrderItemsAsync(itemIds, userId));

            Assert.Equal("Bad Request", exception.Message);
        }

        [Fact]
        public async Task BuyProductsAsync_UpdatesPendingOrdersToDelivered_WhenUserHasPendingOrders()
        {
            // Arrange
            var userId = _userId;

            // Act
            await _orderService.BuyProductsAsync(userId);

            // Assert
            var ordersAfterPurchase = await _context.Orders
                .Where(o => o.UserId == userId)
                .ToListAsync();

            Assert.All(ordersAfterPurchase, order => Assert.Equal(OrderStatus.Delivered, order.Status));
        }

        [Fact]
        public async Task BuyProductsAsync_ThrowsError_WhenUserHasNoPendingOrders()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiExceptionBase>(()
                => _orderService.BuyProductsAsync(userId));

            Assert.Equal("Bad Request", exception.Message);
        }

        [Fact]
        public async Task UpdateOrderItemAmountAsync_SuccessfullyUpdatesAmount_ForFreeProduct()
        {
            // Arrange
            var userId = _userId;
            var dto = new UpdateOrderItemDto
            {
                OrderId = 1,
                ProductId = 1,
                NewAmount = 5
            };

            // Act
            var result = await _orderService.UpdateOrderItemAmountAsync(dto, userId);

            // Assert
            var updatedOrder = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == dto.OrderId);

            var updatedItem = updatedOrder?.OrderItems.FirstOrDefault(oi => oi.ProductId == dto.ProductId);

            Assert.NotNull(updatedItem);
            Assert.Equal(dto.NewAmount, updatedItem.Quantity);
        }

        [Fact]
        public async Task UpdateOrderItemAmountAsync_ThrowsError_WhenOrderNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var dto = new UpdateOrderItemDto
            {
                OrderId = 100,
                ProductId = 1,
                NewAmount = 1000
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiExceptionBase>(()
                => _orderService.UpdateOrderItemAmountAsync(dto, userId));

            Assert.Equal("Not Found", exception.Message);
        }

        [Fact]
        public async Task UpdateOrderItemAmountAsync_ThrowsError_WhenProductNotFound()
        {
            // Arrange
            var userId = _userId;
            var dto = new UpdateOrderItemDto
            {
                OrderId = 5,
                ProductId = 100,
                NewAmount = 5
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiExceptionBase>(()
                => _orderService.UpdateOrderItemAmountAsync(dto, userId));

            Assert.Equal("Not Found", exception.Message);
        }

        [Fact]
        public async Task CreateOrderAsync_CreatesNewOrder_WithValidProductForNewUser()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var product = new Product
            {
                Id = 1,
                Name = "The Witcher 3: Wild Hunt",
                Platform = Platforms.PC
            };

            var createOrderItemDto = new CreateOrderItemDto
            {
                ProductId = product.Id,
                Amount = 1
            };

            // Act
            var result = await _orderService.CreateOrderAsync(createOrderItemDto, userId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Items);
            Assert.Equal(product.Id, result.Items.First().ProductId);
        }

        [Fact]
        public async Task CreateOrderAsync_AddsItemToExistingPendingOrder_WhenOrderExists()
        {
            // Arrange
            var userId = _userId;
            var product = new Product
            {
                Id = 2,
                Name = "Red Dead Redemption 2",
                Platform = Platforms.Nintendo
            };

            var createOrderItemDto = new CreateOrderItemDto
            {
                ProductId = product.Id,
                Amount = 1
            };

            // Act
            var result = await _orderService.CreateOrderAsync(createOrderItemDto, userId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Items.Count > 1);
            Assert.Contains(result.Items, item => item.ProductId == product.Id);
        }

        [Fact]
        public async Task CreateOrderAsync_ThrowsError_WhenProductNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var createOrderItemDto = new CreateOrderItemDto
            {
                ProductId = -1,
                Amount = 1
            };

            // Act & Assert
            var execption = await Assert.ThrowsAsync<ApiExceptionBase>(()
                => _orderService.CreateOrderAsync(createOrderItemDto, userId));

            Assert.Equal("Not Found", execption.Message);
        }
    }
}
