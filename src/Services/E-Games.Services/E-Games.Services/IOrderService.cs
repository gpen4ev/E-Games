using E_Games.Common.DTOs;

namespace E_Games.Services.E_Games.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(CreateOrderItemDto orderItem, Guid userId);

        Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(Guid userId);

        Task<OrderDto> GetOrderByIdAsync(int orderId, Guid userId);

        Task<OrderDto> UpdateOrderItemAmountAsync(UpdateOrderItemDto dto, Guid userId);
    }
}
