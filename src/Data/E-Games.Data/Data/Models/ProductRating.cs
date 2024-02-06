namespace E_Games.Data.Data.Models
{
    public class ProductRating
    {
        public int ProductId { get; set; }

        public Guid UserId { get; set; }

        public int Rating { get; set; }

        public virtual Product? Product { get; set; }

        public virtual ApplicationUser? User { get; set; }
    }
}
