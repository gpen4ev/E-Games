using E_Games.Common;
using Newtonsoft.Json;

namespace E_Games.Web.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ApiExceptionBase ex)
            {
                _logger.LogError(ex, "An API exception occurred.");

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = ex.StatusCode;

                var response = new { message = ex.Message, errors = ex.Errors };
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
