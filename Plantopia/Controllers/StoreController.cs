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

        public IActionResult Index()
        {
            var plants = _context.Plants.ToList();

            var email = HttpContext.Session.GetString("UserEmail");
            var wishlistedIds = new List<int>();

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

            ViewData["WishlistedIds"] = wishlistedIds;
            return View(plants);
        }


        [HttpGet]
        public IActionResult Search(string query)
        {
            // ── kung walay query, balik sa Index ──
            if (string.IsNullOrWhiteSpace(query))
                return RedirectToAction("Index");

            // ── check kung naa bay results sa database ──
            var results = _context.Plants
                .Where(p => p.Name.Contains(query) ||
                            p.Category.Contains(query) ||
                            p.Description.Contains(query))
                .ToList();

            // ── kung walay results, redirect sa Plants with no results flag ──
            if (results.Count == 0)
            {
                TempData["NoResults"] = $"No plants found for \"{query}\".";
                return RedirectToAction("Plants");
            }

            // ── kung naa, redirect sa Plants page with query filter ──
            return RedirectToAction("Plants", new { query = query });
        }

        // ── Update Plants action para mag-accept ug query ──
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

            // ── Limit to 5 kung walay filter (default best sellers view) ──
            if (string.IsNullOrWhiteSpace(query) && string.IsNullOrWhiteSpace(category))
                result = result.Take(5).ToList();
            else if (!string.IsNullOrWhiteSpace(category))
                result = result.Take(6).ToList();

            ViewData["Query"] = query;
            ViewData["Category"] = category;
            return View(result);
        }


        public IActionResult PlantCare()
        {
            return View();
        }

        // ── BestSellers karon nag-query na sa DB ──
        public IActionResult BestSellers()
        {
            var bestSellers = _context.Plants.Take(6).ToList();
            return View(bestSellers);
        }

        public IActionResult NewArrivals()
        {
            return View();
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