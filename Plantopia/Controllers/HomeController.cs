using Microsoft.AspNetCore.Mvc;
using Plantopia.Data;
using Plantopia.Models;

namespace Plantopia.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        [HttpGet("home")]
        public IActionResult Index(string? category = null)
        {
            var activeCategory = category ?? "Indoor Plants";

            var filtered = _context.Plants
                .Where(p => p.Category == activeCategory)
                .ToList();

            var wishlistedIds = new List<int>();
            var email = HttpContext.Session.GetString("UserEmail");
            if (!string.IsNullOrEmpty(email))
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    wishlistedIds = _context.Wishlists
                        .Where(w => w.UserId == user.Id)
                        .Select(w => w.PlantId)
                        .ToList();
                }
            }

            ViewData["ActiveCategory"] = activeCategory;
            ViewData["WishlistedIds"] = wishlistedIds;
            return View(filtered);
        }
    }
}