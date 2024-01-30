using E_Games.Data.Data.Enums;
using System.Text.Json.Serialization;

namespace E_Games.Web.ViewModels
{
    public class UpdateProductModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        [JsonIgnore]
        public string? Logo { get; set; }

        [JsonIgnore]
        public string? Background { get; set; }

        public IFormFile? LogoFile { get; set; }

        public IFormFile? BackgroundImageFile { get; set; }

        public Platforms Platform { get; set; }

        public int TotalRating { get; set; }

        public string? Genre { get; set; }
    }
}
