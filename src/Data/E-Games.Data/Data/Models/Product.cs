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
    }
}
