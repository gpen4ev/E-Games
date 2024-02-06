using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace E_Games.Web.Infrastructure.Filters
{
    public class ValidateSortFilterParamsAttribute : ActionFilterAttribute
    {
        private readonly List<string> _validSortBys = new List<string> { "Rating", "Price" };
        private readonly List<string> _validAgeRanges = new List<string> { "All", "6+", "12+", "18+" };
        private readonly List<string> _validGenres = new List<string> { "Shooter", "Strategy", "Racing", "Fighting" };

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var queryParams = context.HttpContext.Request.Query;
            var sortBy = queryParams["SortBy"].ToString();

            if (!string.IsNullOrEmpty(sortBy) && !_validSortBys.Contains(sortBy))
            {
                context.Result = new BadRequestObjectResult($"Invalid SortBy parameter. Valid parameters are: " +
                    $"{string.Join(", ", _validSortBys)}");
                return;
            }

            var sortOrder = queryParams["SortOrder"].ToString();

            if (!string.IsNullOrEmpty(sortOrder) && sortOrder != "asc" && sortOrder != "desc")
            {
                context.Result = new BadRequestObjectResult("SortOrder parameter must be 'asc' or 'desc'.");
                return;
            }

            var ageRange = queryParams["AgeRange"].ToString();

            if (!string.IsNullOrEmpty(ageRange) && !_validAgeRanges.Contains(ageRange))
            {
                context.Result = new BadRequestObjectResult($"Invalid AgeRange parameter. Valid parameters are: " +
                    $"{string.Join(", ", _validAgeRanges)}");
                return;
            }

            var genres = queryParams["Genres"]
                .ToString()
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            if (genres.Any(genre => !_validGenres.Contains(genre)))
            {
                context.Result = new BadRequestObjectResult($"Invalid Genres parameter. Valid parameters are: " +
                    $"{string.Join(", ", _validGenres)}");
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
