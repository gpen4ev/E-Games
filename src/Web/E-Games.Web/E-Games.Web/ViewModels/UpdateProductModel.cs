using E_Games.Data.Data.Enums;

namespace E_Games.Web.ViewModels
{
    /// <summary>
    /// Update Product model
    /// </summary>
    public class UpdateProductModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public IFormFile? LogoFile { get; set; }

        public IFormFile? BackgroundImageFile { get; set; }

        public Platforms Platform { get; set; }

        public int TotalRating { get; set; }

        public string? Genre { get; set; }

        public Rating Rating { get; set; }

        public double Price { get; set; }

        public int Count { get; set; }
    }
}
