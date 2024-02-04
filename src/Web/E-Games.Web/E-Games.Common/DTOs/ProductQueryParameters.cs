namespace E_Games.Common.DTOs
{
    public class ProductQueryParameters
    {
        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public List<string> Genres { get; set; } = new List<string>();

        public string AgeRange { get; set; } = "All";

        public string SortBy { get; set; } = "Rating";

        public string SortOrder { get; set; } = "asc";
    }
}
