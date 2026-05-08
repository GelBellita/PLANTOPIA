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
                new Plant { Id = 1,  Name = "Money Tree",    Category = "Indoor Plants", Badge = "New", Price = 280, ImageUrl = "/images/Money Tree.png" },
                new Plant { Id = 2,  Name = "Philodendron",  Category = "Indoor Plants", Badge = "Hot", Price = 260, ImageUrl = "/images/Philodendron.png" },
                new Plant { Id = 3,  Name = "ZZ Plant",      Category = "Indoor Plants", Badge = "New", Price = 280, ImageUrl = "/images/ZZ Plant.png" },
                new Plant { Id = 4,  Name = "Snake Plant",   Category = "Indoor Plants", Badge = "Hot", Price = 320, ImageUrl = "/images/Snake Plant.png" },
                new Plant { Id = 5,  Name = "Fortune Plant", Category = "Outdoor Plants", Badge = "New", Price = 199, ImageUrl = "/images/Fortune Plant.png" },
                new Plant { Id = 6,  Name = "Areca Palm",    Category = "Outdoor Plants", Badge = "Hot", Price = 399, ImageUrl = "/images/Areca Palm.png" },
                new Plant { Id = 7,  Name = "Santan",        Category = "Outdoor Plants", Badge = "New", Price = 149, ImageUrl = "/images/Santan.png" },
                new Plant { Id = 8,  Name = "Gumamela",      Category = "Outdoor Plants", Badge = "Hot", Price = 149, ImageUrl = "/images/Gumamela.png" },
                new Plant { Id = 9,  Name = "Caladium",           Category = "Balcony / Patio", Badge = "Hot", Price = 260, ImageUrl = "/images/caladium.jpg" },
                new Plant { Id = 14, Name = "Sampaguita",         Category = "Balcony / Patio", Badge = "New", Price = 175, ImageUrl = "/images/sampaguita.jpg" },
                new Plant { Id = 10, Name = "Pandan Plant",       Category = "Balcony / Patio", Badge = "New", Price = 150, ImageUrl = "/images/pandan.jpg" },
                new Plant { Id = 15, Name = "Panda Plant",           Category = "Succulents",      Badge = "New", Price = 280, ImageUrl = "/images/Panda Plant.png" },
                new Plant { Id = 16, Name = "Golden Barrel Cactus",  Category = "Succulents",      Badge = "Hot", Price = 320, ImageUrl = "/images/Golden Barrel Cactus.png" },
                new Plant { Id = 17, Name = "Haworthia",             Category = "Succulents",      Badge = "Hot", Price = 260, ImageUrl = "/images/Haworthia.png" },
                new Plant { Id = 18, Name = "Echeveria",             Category = "Succulents",      Badge = "New", Price = 300, ImageUrl = "/images/Echeveria.png" },
                new Plant { Id = 19, Name = "Turtle Vine",           Category = "Hanging Plants",  Badge = "New", Price = 280, ImageUrl = "/images/Turtle Vine.png" },
                new Plant { Id = 20, Name = "Spider Plant",          Category = "Hanging Plants",  Badge = "Hot", Price = 260, ImageUrl = "/images/Spider Plant.png" },
                new Plant { Id = 21, Name = "English Ivy",           Category = "Hanging Plants",  Badge = "Hot", Price = 300, ImageUrl = "/images/English Ivy.png" },
                new Plant { Id = 22, Name = "Boston Fern",           Category = "Hanging Plants",  Badge = "New", Price = 320, ImageUrl = "/images/Boston Fern.png" },
            };

            var filtered = string.IsNullOrEmpty(category)
                ? allPlants.Where(p => p.Category == "Indoor Plants").ToList()
                : allPlants.Where(p => p.Category == category).ToList();

            ViewData["ActiveCategory"] = category ?? "Indoor Plants";
            return View(filtered);
        }
    }
}