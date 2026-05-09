using Microsoft.AspNetCore.Mvc;
using Plantopia.Data;
using Plantopia.Models;

namespace Plantopia.Controllers
{
    public class StoreController : Controller
    {
        private readonly AppDbContext _context;

        public StoreController(AppDbContext context)
        {
            _context = context;
        }

        // ── Helper to get wishlisted plant IDs for current user ──
        private List<int> GetWishlistedIds()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email)) return new List<int>();
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return new List<int>();
            return _context.Wishlists
                .Where(w => w.UserId == user.Id)
                .Select(w => w.PlantId)
                .ToList();
        }

        public IActionResult Index()
        {
            var plants = _context.Plants.ToList();
            ViewData["WishlistedIds"] = GetWishlistedIds();
            return View(plants);
        }

        [HttpGet]
        public IActionResult Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return RedirectToAction("Index");

            var results = _context.Plants
                .Where(p => p.Name.Contains(query) ||
                            p.Category.Contains(query) ||
                            p.Description.Contains(query))
                .ToList();

            if (results.Count == 0)
            {
                TempData["NoResults"] = $"No plants found for \"{query}\".";
                return RedirectToAction("Plants");
            }

            return RedirectToAction("Plants", new { query = query });
        }

        public IActionResult Plants(string query, string category)
        {
            var plants = _context.Plants.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
                plants = plants.Where(p => p.Name.Contains(query) ||
                                           p.Category.Contains(query) ||
                                           p.Description.Contains(query));

            if (!string.IsNullOrWhiteSpace(category))
                plants = plants.Where(p => p.Category == category);

            var result = plants.ToList();

            if (string.IsNullOrWhiteSpace(query) && string.IsNullOrWhiteSpace(category))
                result = result.Take(5).ToList();
            else if (!string.IsNullOrWhiteSpace(category))
                result = result.Take(6).ToList();

            ViewData["Query"] = query;
            ViewData["Category"] = category;
            ViewData["WishlistedIds"] = GetWishlistedIds();
            return View(result);
        }

        public IActionResult PlantCare()
        {
            return View();
        }

        public IActionResult BestSellers()
        {
            var plants = _context.Plants.ToList();
            ViewData["WishlistedIds"] = GetWishlistedIds();
            return View(plants);
        }

        public IActionResult NewArrivals()
        {
            var plants = _context.Plants
                .OrderByDescending(p => p.Id)
                .ToList();
            ViewData["WishlistedIds"] = GetWishlistedIds();
            return View(plants);
        }

        public IActionResult Sale()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return RedirectToAction("Profile", "Account");
        }

        public IActionResult Purchases()
        {
            return RedirectToAction("Purchases", "Account");
        }

        public IActionResult Wishlist()
        {
            return RedirectToAction("Wishlist", "Account");
        }

        public IActionResult Messages()
        {
            return RedirectToAction("Messages", "Account");
        }

        public IActionResult Cart()
        {
            return RedirectToAction("Cart", "Account");
        }
    }
}