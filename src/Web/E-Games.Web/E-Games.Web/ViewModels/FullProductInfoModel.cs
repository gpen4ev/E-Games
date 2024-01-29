using E_Games.Data.Data.Enums;

namespace E_Games.Web.ViewModels
{
    public class FullProductInfoModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public Platforms Platform { get; set; }

        public DateTime DateCreated { get; set; }

        public int TotalRating { get; set; }

        public string? Genre { get; set; }

        public Rating Rating { get; set; }

        public string? Logo { get; set; }

        public string? Background { get; set; }

        public double Price { get; set; }

        public int Count { get; set; }
    }
}
