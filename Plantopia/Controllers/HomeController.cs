using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Plantopia.Models;

namespace Plantopia.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet("")]
        [HttpGet("home")]
        public IActionResult Index()
        {
            var plants = new List<Plant>
            {
                new Plant
                {
                    Id       = 1,
                    Name     = "Succulent Trio Set",
                    Category = "Indoor Plants",
                    Badge    = "New",
                    Price    = 280,
                    ImageUrl = "/images/succulent.jpg"
                },
                new Plant
                {
                    Id       = 2,
                    Name     = "Snake Plant",
                    Category = "Indoor Plants",
                    Badge    = "Hot",
                    Price    = 260,
                    ImageUrl = "/images/snake.jpg"
                },
                new Plant
                {
                    Id       = 3,
                    Name     = "Monstera Plant",
                    Category = "Indoor Plants",
                    Badge    = "",
                    Price    = 280,
                    ImageUrl = "/images/monstera.jpg"
                },
                new Plant
                {
                    Id       = 4,
                    Name     = "Santan",
                    Category = "Outdoor Plants",
                    Badge    = "",
                    Price    = 280,
                    ImageUrl = "/images/santan.jpg"
                },
            };
            return View(plants);
        }

        [HttpGet("about")]
        public IActionResult About() => RedirectToAction(nameof(Index));

        [HttpGet("services")]
        public IActionResult Services() => RedirectToAction(nameof(Index));

        [HttpGet("contact")]
        public IActionResult Contact() => View();
    }
}