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
        public IActionResult Index(string? category = null)
        {
            var allPlants = new List<Plant>
    {
        new Plant { Id = 1, Name = "Succulent Trio Set", Category = "Indoor Plants",   Badge = "New", Price = 280, ImageUrl = "/images/succulent.jpg" },
        new Plant { Id = 2, Name = "Snake Plant",        Category = "Indoor Plants",   Badge = "Hot", Price = 260, ImageUrl = "/images/snake.jpg" },
        new Plant { Id = 3, Name = "Monstera Plant",     Category = "Indoor Plants",   Badge = "New", Price = 280, ImageUrl = "/images/monstera.jpg" },
        new Plant { Id = 11, Name = "Peace Lily",        Category = "Indoor Plants",   Badge = "New", Price = 320, ImageUrl = "/images/peace-lily.jpg" },
        new Plant { Id = 4, Name = "Santan",             Category = "Outdoor Plants",  Badge = "New", Price = 280, ImageUrl = "/images/santan.jpg" },
        new Plant { Id = 5,  Name = "Bougainvillea",     Category = "Outdoor Plants",  Badge = "New", Price = 320, ImageUrl = "/images/Bougainvillea.jpg" },
        new Plant { Id = 6,  Name = "Ylang-Ylang",       Category = "Outdoor Plants",  Badge = "New", Price = 450, ImageUrl = "/images/ylang-ylang.jpg" },
        new Plant { Id = 7,  Name = "Gumamela",          Category = "Outdoor Plants",  Badge = "Hot", Price = 180, ImageUrl = "/images/gumamela.jpeg" },
        new Plant { Id = 8,  Name = "Dwarf Ixora",       Category = "Balcony / Patio", Badge = "New", Price = 220, ImageUrl = "/images/dwarf-ixora.jpg" },
        new Plant { Id = 9,  Name = "Caladium",          Category = "Balcony / Patio", Badge = "Hot", Price = 260, ImageUrl = "/images/caladium.jpg" },
        new Plant { Id = 14, Name = "Sampaguita",        Category = "Balcony / Patio", Badge = "New", Price = 175, ImageUrl = "/images/sampaguita.jpg" },
        new Plant { Id = 10, Name = "Pandan Plant",      Category = "Balcony / Patio", Badge = "New", Price = 150, ImageUrl = "/images/pandan.jpg" },

    };

            var filtered = string.IsNullOrEmpty(category)
                ? allPlants.Where(p => p.Category == "Indoor Plants").ToList()
                : allPlants.Where(p => p.Category == category).ToList();

            ViewData["ActiveCategory"] = category ?? "Indoor Plants";
            return View(filtered);
        }

        [HttpGet("about")]
        public IActionResult About() => Redirect("/#about");

        [HttpGet("sellers")]
        public IActionResult Sellers() => Redirect("/#sellers");

        [HttpGet("contact")]
        public IActionResult Contact() => View();
    }
}
