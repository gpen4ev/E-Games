namespace E_Games.Common.DTOs
{
    public class PagedResult<T>
    {
        public IEnumerable<T>? Items { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public int TotalPages { get; set; }
    }
}