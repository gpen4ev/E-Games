using E_Games.Data.Data.Enums;
using Microsoft.AspNetCore.Http;

namespace E_Games.Common.DTOs
{
    public class CreateProductDto
    {
        public string? Name { get; set; }

        public string? Logo { get; set; }

        public string? Background { get; set; }

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
