namespace E_Games.Common.DTOs
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public int Amount { get; set; }

        public decimal Price { get; set; }
    }
}
