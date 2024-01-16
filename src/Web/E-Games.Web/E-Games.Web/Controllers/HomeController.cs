using Microsoft.AspNetCore.Mvc;

namespace E_Games.Web.Controllers
{
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

        [HttpGet("/Home/GetInfo")]
        public string GetInfo()
        {
            _logger.LogInformation("GetInfo action was called!");

            return "Hello world";
        }
    }
}
