using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Games.Web.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/")]
        public string Index()
        {
            return "Welcome to the Home page!";
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("/Home/GetInfo")]
        public string GetInfo()
        {
            _logger.LogInformation("GetInfo action was called!");

            return "Hello world";
        }
    }
}

/*
 Redirecting issue when using an Authorize attribute workaround
 https://github.com/dotnet/aspnetcore/issues/9039
*/