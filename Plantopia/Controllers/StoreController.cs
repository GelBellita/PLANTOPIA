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
            return View();
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
        public IActionResult Plants(string query)
        {
            var plants = string.IsNullOrWhiteSpace(query)
                ? _context.Plants.ToList()
                : _context.Plants
                    .Where(p => p.Name.Contains(query) ||
                                p.Category.Contains(query) ||
                                p.Description.Contains(query))
                    .ToList();

            ViewData["Query"] = query;
            return View(plants);
        }
        public IActionResult PotsAndPlanters()
        {
            return View();
        }

        public IActionResult PlantCare()
        {
            return View();
        }

        public IActionResult Accessories()
        {
            return View();
        }

        public IActionResult BestSellers()
        {
            return View();
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