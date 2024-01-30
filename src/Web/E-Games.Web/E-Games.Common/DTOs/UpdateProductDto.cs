using E_Games.Data.Data.Enums;

namespace E_Games.Common.DTOs
{
    public class UpdateProductDto
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Logo { get; set; }

        public string? Background { get; set; }

        public Platforms Platform { get; set; }

        public int TotalRating { get; set; }

        public string? Genre { get; set; }
    }
}
