using AutoMapper;
using E_Games.Common.DTOs;
using E_Games.Data.Data;
using E_Games.Data.Data.Enums;
using E_Games.Data.Data.Models;
using E_Games.Web.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace E_Games.Services.E_Games.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderItemDto model, Guid userId)
        {
            var product = await _context.Products.FindAsync(model.ProductId);
            if (product == null)
            {
                ErrorResponseHelper.RaiseError(ErrorMessage.NotFound, "Product not found");
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == OrderStatus.Pending);
            if (order == null)
            {
                order = new Order
                {
                    UserId = userId,
                    CreationDate = DateTime.UtcNow,
                    Status = OrderStatus.Pending
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
            }

            var orderItem = new OrderItem
            {
                OrderId = order.OrderId,
                ProductId = model.ProductId,
                Quantity = model.Amount,
                Price = (decimal)product!.Price
            };

            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();

            var orderViewModel = _mapper.Map<OrderDto>(order);

            orderViewModel.Items = _context.OrderItems
                .Where(oi => oi.OrderId == order.OrderId)
                .Select(oi => _mapper.Map<OrderItemDto>(oi)).ToList();

            return orderViewModel;
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId, Guid userId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null)
            {
                return null!;
            }

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(Guid userId)
        {
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<OrderDto> UpdateOrderItemAmountAsync(UpdateOrderItemDto dto, Guid userId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == dto.OrderId && o.UserId == userId);

            if (order == null)
            {
                ErrorResponseHelper.RaiseError(ErrorMessage.NotFound, "Order not found");
            }

            var orderItem = order!.OrderItems.FirstOrDefault(oi => oi.ProductId == dto.ProductId);

            if (orderItem == null)
            {
                ErrorResponseHelper.RaiseError(ErrorMessage.NotFound, "Product not found in the order.");
            }

            if (orderItem!.Price != 0)
            {
                ErrorResponseHelper.RaiseError(ErrorMessage.BadRequest, "Can only change the amount for unpaid (free) products.");
            }

            orderItem.Quantity = dto.NewAmount;

            await _context.SaveChangesAsync();

            return _mapper.Map<OrderDto>(order);
        }

        public async Task DeleteOrderItemsAsync(IEnumerable<int> itemIds, Guid userId)
        {
            var orderItems = await _context.OrderItems
                .Where(oi => itemIds.Contains(oi.OrderId) && oi.Order!.UserId == userId)
                .ToListAsync();

            if (!orderItems.Any())
            {
                ErrorResponseHelper.RaiseError(ErrorMessage.BadRequest, "No matching order items found for deletion.");
            }

            _context.OrderItems.RemoveRange(orderItems);

            await _context.SaveChangesAsync();
        }

        public async Task BuyProductsAsync(Guid userId)
        {
            var ordersToBuy = await _context.Orders
                .Where(o => o.UserId == userId && o.Status == OrderStatus.Pending)
                .ToListAsync();

            if (!ordersToBuy.Any())
            {
                ErrorResponseHelper.RaiseError(ErrorMessage.BadRequest, "There are no pending orders to buy.");
            }

            foreach (var order in ordersToBuy)
            {
                // Delivered is used as Bought/Completed
                order.Status = OrderStatus.Delivered;
            }

            await _context.SaveChangesAsync();
        }
    }
}
