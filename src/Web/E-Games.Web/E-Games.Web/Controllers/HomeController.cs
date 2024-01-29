using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Games.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Home Page.
        /// </summary>
        [HttpGet("/")]
        public string Index()
        {
            return "Welcome to the Home page!";
        }

        /// <summary>
        /// Get Info page restricted to Admins only.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("/Home/GetInfo")]
        public string GetInfo()
        {
            _logger.LogInformation("GetInfo action was called!");

            return "Hello world";
        }
    }
}