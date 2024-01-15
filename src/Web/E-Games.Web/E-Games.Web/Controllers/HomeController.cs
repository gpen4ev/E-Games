using Microsoft.AspNetCore.Mvc;

namespace E_Games.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        [HttpGet("/")]
        public string Index()
        {
            return "Welcome to the Home page!";
        }

        [HttpGet("/Home/GetInfo")]
        public string GetInfo()
        {
            this.logger.LogInformation("GetInfo action was called!");

            return "Hello world";
        }
    }
}
