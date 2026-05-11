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

            var emailP = HttpContext.Session.GetString("UserEmail");
            var wishlistedIdsP = new List<int>();
            if (!string.IsNullOrEmpty(emailP))
            {
                var userP = _context.Users.FirstOrDefault(u => u.Email == emailP);
                if (userP != null)
                    wishlistedIdsP = _context.Wishlists
                        .Where(w => w.UserId == userP.Id)
                        .Select(w => w.PlantId).ToList();
            }
            ViewData["WishlistedIds"] = wishlistedIdsP;
            ViewData["Query"] = query;
            ViewData["Category"] = category;
            return View(result);
        }


        public IActionResult PlantCare()
        {
            return View();
        }

        public IActionResult BestSellers(string sort = "bestselling")
        {
            var query = _context.Plants
                .Where(p => p.Tags.Contains("BestSeller"))
                .AsQueryable();

            query = sort switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "top_rated" => query.OrderByDescending(p => p.Rating),
                _ => query.OrderByDescending(p => p.Id)
            };

            ViewData["CurrentSort"] = sort;

            var email = HttpContext.Session.GetString("UserEmail");
            var wishlistedIds = new List<int>();
            if (!string.IsNullOrEmpty(email))
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                    wishlistedIds = _context.Wishlists
                        .Where(w => w.UserId == user.Id)
                        .Select(w => w.PlantId).ToList();
            }
            ViewData["WishlistedIds"] = wishlistedIds;

            return View(query.ToList());
        }


        public IActionResult NewArrivals(string sort = "newest")
        {
            var query = _context.Plants
                .Where(p => p.Tags.Contains("NewArrival"))
                .AsQueryable();

            query = sort switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "bestselling" => query.OrderByDescending(p => p.Id),
                _ => query.OrderByDescending(p => p.Id)
            };

            ViewData["CurrentSort"] = sort;

            var email = HttpContext.Session.GetString("UserEmail");
            var wishlistedIds = new List<int>();
            if (!string.IsNullOrEmpty(email))
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                    wishlistedIds = _context.Wishlists
                        .Where(w => w.UserId == user.Id)
                        .Select(w => w.PlantId).ToList();
            }
            ViewData["WishlistedIds"] = wishlistedIds;

            return View(query.ToList());
        }


        public IActionResult Sale(string sort = "featured")
        {
            var query = _context.Plants
                .Where(p => p.Tags.Contains("Sale"))
                .AsQueryable();

            query = sort switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "biggest_discount" => query.OrderByDescending(p => p.DiscountPercent),
                _ => query.OrderByDescending(p => p.Id)
            };

            ViewData["CurrentSort"] = sort;

            var email = HttpContext.Session.GetString("UserEmail");
            var wishlistedIds = new List<int>();
            if (!string.IsNullOrEmpty(email))
            {
                var user = _context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                    wishlistedIds = _context.Wishlists
                        .Where(w => w.UserId == user.Id)
                        .Select(w => w.PlantId).ToList();
            }
            ViewData["WishlistedIds"] = wishlistedIds;

            return View(query.ToList());
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

        // ── FIX: accept selectedIds and filter cart items accordingly ──
        public IActionResult Checkout(string selectedIds = "")
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Auth");

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return RedirectToAction("Login", "Auth");

            // Parse the comma-separated cart item IDs sent from the cart page
            var selectedIdList = selectedIds
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.TryParse(s.Trim(), out var id) ? id : -1)
                .Where(id => id > 0)
                .ToHashSet();

            // Build the cart query — filter to selected items only
            var query = _context.CartItems.Where(c => c.UserId == user.Id);
            if (selectedIdList.Any())
                query = query.Where(c => selectedIdList.Contains(c.Id));

            var cartItems = query
                .Select(c => new CartItemViewModel
                {
                    CartItemId = c.Id,
                    PlantId = c.PlantId,
                    Name = c.Plant.Name,
                    Price = c.Plant.DiscountPercent.HasValue
                        ? c.Plant.Price * (1 - c.Plant.DiscountPercent.Value / 100m)
                        : c.Plant.Price,
                    OriginalPrice = c.Plant.Price,
                    ImageUrl = c.Plant.ImageUrl,
                    Quantity = c.Quantity,
                    DiscountPercent = c.Plant.DiscountPercent
                })
                .ToList();

            if (!cartItems.Any())
                return RedirectToAction("Cart", "Account");

            // Compute totals from selected items only
            decimal subtotal = cartItems.Sum(c => c.Price * c.Quantity);
            decimal shippingFee = subtotal >= 4000 ? 0 : 150;
            decimal total = subtotal + shippingFee;

            var defaultAddress = _context.Addresses
                .FirstOrDefault(a => a.UserId == user.Id && a.IsDefault)
                ?? _context.Addresses.FirstOrDefault(a => a.UserId == user.Id);

            ViewData["User"] = user;
            ViewData["DefaultAddress"] = defaultAddress;
            ViewData["Subtotal"] = subtotal;
            ViewData["ShippingFee"] = shippingFee;
            ViewData["Total"] = total;
            ViewData["ItemCount"] = cartItems.Sum(c => c.Quantity);

            return View(cartItems);
        }
    }
}