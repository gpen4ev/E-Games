using E_Games.Data.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace E_Games.Data.Data.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        [EnumDataType(typeof(Platforms))]
        public Platforms Platform { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        public int TotalRating { get; set; }

        public string? Genre { get; set; }

        public Rating Rating { get; set; }

        public string? Logo { get; set; }

        public string? Background { get; set; }

        public double Price { get; set; }

        public int Count { get; set; }

        public virtual ICollection<ProductRating> Ratings { get; set; } = new List<ProductRating>();
    }
}
