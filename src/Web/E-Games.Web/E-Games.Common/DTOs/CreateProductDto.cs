using E_Games.Data.Data.Enums;

namespace E_Games.Common.DTOs
{
    public class CreateProductDto
    {
        public string? Name { get; set; }

        public Platforms Platform { get; set; }

        public DateTime DateCreated { get; set; }

        public int TotalRating { get; set; }

        public string? Genre { get; set; }
    }
}
