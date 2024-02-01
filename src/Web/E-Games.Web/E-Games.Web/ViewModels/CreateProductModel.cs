using E_Games.Data.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace E_Games.Web.ViewModels
{
    /// <summary>
    /// Create Product model
    /// </summary>
    public class CreateProductModel
    {
        /// <summary>
        /// Name of a product.
        /// Name is a required field
        /// </summary>
        /// <example>FIFA 2024</example>
        [Required]
        public string? Name { get; set; }

        public IFormFile? LogoFile { get; set; }

        public IFormFile? BackgroundImageFile { get; set; }

        public Platforms Platform { get; set; }

        public DateTime DateCreated { get; set; }

        public int TotalRating { get; set; }

        public string? Genre { get; set; }

        public Rating Rating { get; set; }

        public double Price { get; set; }

        public int Count { get; set; }
    }
}
