namespace E_Games.Common.DTOs
{
    public class UpdateOrderItemDto
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int NewAmount { get; set; }
    }
}
