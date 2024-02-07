using E_Games.Data.Data.Enums;

namespace E_Games.Data.Data.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public DateTime CreationDate { get; set; }

        public int Amount { get; set; }

        public Guid UserId { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
