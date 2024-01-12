using Microsoft.AspNetCore.Mvc;

namespace E_Games.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() {
            return Content("Welcome to the Home page!");
        }

        [HttpGet("/Home/GetInfo")]
        public IActionResult GetInfo()
        {
            return Content("Hello world");
        }
    }
}
