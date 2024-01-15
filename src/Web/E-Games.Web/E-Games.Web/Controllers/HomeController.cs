using Microsoft.AspNetCore.Mvc;

namespace E_Games.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public string Index()
        {
            return "Welcome to the Home page!";
        }

        [HttpGet("/Home/GetInfo")]
        public string GetInfo()
        {
            return "Hello world";
        }
    }
}
