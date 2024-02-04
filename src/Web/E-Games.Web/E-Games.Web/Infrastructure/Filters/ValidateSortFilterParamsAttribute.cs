using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace E_Games.Web.Infrastructure.Filters
{
    public class ValidateSortFilterParamsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var queryParams = context.HttpContext.Request.Query;

            var sortBy = queryParams["SortBy"].ToString();
            var validSortBy = new List<string> { "Rating", "Price" };

            if (!string.IsNullOrEmpty(sortBy) && !validSortBy.Contains(sortBy))
            {
                context.Result = new BadRequestObjectResult($"Invalid SortBy parameter. Valid parameters are: " +
                    $"{string.Join(", ", validSortBy)}");
                return;
            }

            var sortOrder = queryParams["SortOrder"].ToString();

            if (!string.IsNullOrEmpty(sortOrder) && sortOrder != "asc" && sortOrder != "desc")
            {
                context.Result = new BadRequestObjectResult("SortOrder parameter must be 'asc' or 'desc'.");
                return;
            }

            // TODO: add validation for filter by age and genres

            base.OnActionExecuting(context);
        }
    }
}
