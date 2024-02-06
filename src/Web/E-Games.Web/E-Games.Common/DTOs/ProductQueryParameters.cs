namespace E_Games.Common.DTOs
{
    /// <summary>
    /// Represents the parameters for querying products with support for pagination, filtering by genre and age range, and sorting.
    /// </summary>
    public class ProductQueryParameters
    {
        /// <summary>
        /// Gets or sets the page number for pagination. Default is 1.
        /// </summary>
        /// <example>For the first page, use 1.</example>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of items per page for pagination. Default is 10.
        /// </summary>
        /// <example>To retrieve 10 items per page, use 10.</example>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Gets or sets the list of genres to filter the products. An empty list applies no genre filter.
        /// </summary>
        /// <example>To filter by strategy and action genres, use ["Strategy", "Action"].</example>
        public List<string> Genres { get; set; } = new List<string>();

        /// <summary>
        /// Gets or sets the age range for filtering products. Default is "All", applying no age filter.
        /// Supported values: "All", "6+", "12+", "18+".
        /// </summary>
        /// <example>To filter products suitable for ages 12 and above, use "12+".</example>
        public string AgeRange { get; set; } = "All";

        /// <summary>
        /// Gets or sets the property name to sort the products. Default is "Rating".
        /// Supported values: "Rating", "Price".
        /// </summary>
        /// <example>To sort products by price, use "Price".</example>
        public string SortBy { get; set; } = "Rating";

        /// <summary>
        /// Gets or sets the sort order. Default is "asc" for ascending order.
        /// Supported values: "asc", "desc".
        /// </summary>
        /// <example>To sort in descending order, use "desc".</example>
        public string SortOrder { get; set; } = "asc";
    }
}
