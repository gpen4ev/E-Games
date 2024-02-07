using E_Games.Data.Data.Enums;

namespace E_Games.Common.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }

        public DateTime CreationDate { get; set; }

        public OrderStatus Status { get; set; }

        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    }
}
