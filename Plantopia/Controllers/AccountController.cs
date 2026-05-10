using Microsoft.AspNetCore.Mvc;
using Plantopia.Data;
using Plantopia.Models;

namespace Plantopia.Controllers
{
    public class AccountController : Controller
    {
        // ── Dependency Injection 
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Profile()
        {
            // ── Get logged in user from session ──
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Auth");

            // ── LINQ query 
            var user = _context.Users
                .FirstOrDefault(u => u.Email == email);

            if (user == null)
                return RedirectToAction("Login", "Auth");

            ViewData["Title"] = "My Profile";
            return View(user);
        }

        // ── HttpPost — Save Changes ──
        [HttpPost]
        public IActionResult UpdateProfile(User model)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Auth");

            var user = _context.Users
                .FirstOrDefault(u => u.Email == email);

            if (user == null)
                return RedirectToAction("Login", "Auth");

            // ── Update fields — use empty string fallback to prevent NULL errors ──
            user.FullName = string.IsNullOrWhiteSpace(model.FullName) ? user.FullName : model.FullName;
            user.Phone = model.Phone ?? "";
            user.Location = model.Location ?? "";

            _context.Users.Update(user);
            _context.SaveChanges();

            // ── Update session so topbar name reflects changes immediately ──
            HttpContext.Session.SetString("UserName", user.FullName);

            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile photo)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Auth");

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return RedirectToAction("Login", "Auth");

            if (photo != null && photo.Length > 0)
            {
                // ── Save file to wwwroot/images/profiles/ ──
                var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profiles");
                Directory.CreateDirectory(folder);

                var fileName = $"{user.Id}_{Path.GetFileName(photo.FileName)}";
                var filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                // ── Update database  ──
                user.ProfilePhoto = $"/images/profiles/{fileName}";
                _context.Users.Update(user);
                _context.SaveChanges();

                TempData["Success"] = "Profile photo updated!";
            }

            return RedirectToAction("Profile");
        }

        public IActionResult Purchases()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Auth");

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return RedirectToAction("Login", "Auth");

            var orders = _context.Orders
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.OrderedAt)
                .Select(o => new
                {
                    o.Id,
                    o.Status,
                    o.Total,
                    o.OrderedAt,
                    Items = _context.OrderItems
                        .Where(i => i.OrderId == o.Id)
                        .Select(i => new { i.PlantName, i.PlantImageUrl, i.Quantity })
                        .ToList()
                })
                .ToList();

            ViewData["Title"] = "My Purchase";
            return View(orders.Select(o => new Plantopia.Models.OrderSummaryViewModel
            {
                Id = o.Id,
                Status = o.Status,
                Total = o.Total,
                OrderedAt = o.OrderedAt,
                Items = o.Items.Select(i => new Plantopia.Models.OrderItemSummary
                {
                    PlantName = i.PlantName,
                    PlantImageUrl = i.PlantImageUrl,
                    Quantity = i.Quantity
                }).ToList()
            }).ToList());
        }

        public IActionResult Wishlist()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Auth");

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return RedirectToAction("Login", "Auth");

            var wishlistItems = _context.Wishlists
                .Where(w => w.UserId == user.Id)
                .Select(w => w.Plant)
                .ToList();

            ViewData["Title"] = "My Wishlist";
            return View(wishlistItems);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult ToggleWishlist(int plantId)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return Json(new { success = false, message = "Not logged in" });

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return Json(new { success = false, message = "User not found" });

            var existing = _context.Wishlists
                .FirstOrDefault(w => w.UserId == user.Id && w.PlantId == plantId);

            if (existing != null)
            {
                _context.Wishlists.Remove(existing);
                try
                {
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    // Already removed — treat as success
                }
                return Json(new { success = true, wishlisted = false });
            }
            else
            {
                try
                {
                    _context.Wishlists.Add(new Wishlist
                    {
                        UserId = user.Id,
                        PlantId = plantId,
                        AddedAt = DateTime.Now
                    });
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    // Already exists — treat as success
                }
                return Json(new { success = true, wishlisted = true });
            }
        }

        public IActionResult Messages()
        {
            ViewData["Title"] = "Messages";
            return View();
        }

        public IActionResult Cart()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Auth");

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return RedirectToAction("Login", "Auth");

            var cartItems = _context.CartItems
                .Where(c => c.UserId == user.Id)
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

            decimal subtotal = cartItems.Sum(c => c.Price * c.Quantity);
            decimal shippingFee = subtotal >= 4000 ? 0 : (subtotal > 0 ? 150 : 0);
            decimal total = subtotal + shippingFee;
            decimal amountToFree = Math.Max(0, 4000 - subtotal);
            decimal freeProgress = Math.Min(100, (subtotal / 4000m) * 100);

            ViewData["Title"] = "My Cart";
            ViewData["Subtotal"] = subtotal;
            ViewData["ShippingFee"] = shippingFee;
            ViewData["Total"] = total;
            ViewData["ItemCount"] = cartItems.Sum(c => c.Quantity);
            ViewData["AmountToFreeShipping"] = amountToFree;
            ViewData["FreeShippingProgress"] = freeProgress;

            return View(cartItems);
        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult AddToCart(int plantId, int quantity = 1)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return Json(new { success = false, redirect = "/Auth/Login" });

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return Json(new { success = false });

            var plant = _context.Plants.FirstOrDefault(p => p.Id == plantId);
            if (plant == null)
                return Json(new { success = false, message = "Plant not found" });

            var existing = _context.CartItems
                .FirstOrDefault(c => c.UserId == user.Id && c.PlantId == plantId);

            if (existing != null)
            {
                existing.Quantity += quantity;
                _context.CartItems.Update(existing);
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    UserId = user.Id,
                    PlantId = plantId,
                    Quantity = quantity,
                    AddedAt = DateTime.Now
                });
            }

            _context.SaveChanges();

            var cartCount = _context.CartItems
                .Where(c => c.UserId == user.Id)
                .Sum(c => c.Quantity);

            return Json(new { success = true, cartCount });
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult UpdateCartQuantity(int cartItemId, int quantity)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return Json(new { success = false });

            var item = _context.CartItems
                .FirstOrDefault(c => c.Id == cartItemId && c.UserId == user.Id);

            if (item == null) return Json(new { success = false });

            if (quantity <= 0)
                _context.CartItems.Remove(item);
            else
            {
                item.Quantity = quantity;
                _context.CartItems.Update(item);
            }

            _context.SaveChanges();

            var cartItems = _context.CartItems
                .Where(c => c.UserId == user.Id)
                .Select(c => new { c.Quantity, c.Plant.Price, c.Plant.DiscountPercent })
                .ToList();

            decimal subtotal = cartItems.Sum(c =>
            {
                decimal ep = c.DiscountPercent.HasValue
                    ? c.Price * (1 - c.DiscountPercent.Value / 100m)
                    : c.Price;
                return ep * c.Quantity;
            });

            decimal shippingFee = subtotal >= 4000 ? 0 : (subtotal > 0 ? 150 : 0);
            decimal total = subtotal + shippingFee;
            int cartCount = cartItems.Sum(c => c.Quantity);

            return Json(new
            {
                success = true,
                subtotal = subtotal.ToString("N2"),
                shippingFee = shippingFee.ToString("N2"),
                total = total.ToString("N2"),
                cartCount,
                amountToFreeShipping = Math.Max(0, 4000 - subtotal).ToString("N2"),
                freeShippingProgress = (int)Math.Min(100, (subtotal / 4000m) * 100)
            });
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult RemoveFromCart(int cartItemId)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return Json(new { success = false });

            var item = _context.CartItems
                .FirstOrDefault(c => c.Id == cartItemId && c.UserId == user.Id);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                _context.SaveChanges();
            }

            var cartItems = _context.CartItems
                .Where(c => c.UserId == user.Id)
                .Select(c => new { c.Quantity, c.Plant.Price, c.Plant.DiscountPercent })
                .ToList();

            decimal subtotal = cartItems.Sum(c =>
            {
                decimal ep = c.DiscountPercent.HasValue
                    ? c.Price * (1 - c.DiscountPercent.Value / 100m)
                    : c.Price;
                return ep * c.Quantity;
            });

            decimal shippingFee = subtotal >= 4000 ? 0 : (subtotal > 0 ? 150 : 0);
            decimal total = subtotal + shippingFee;
            int cartCount = cartItems.Sum(c => c.Quantity);

            return Json(new
            {
                success = true,
                subtotal = subtotal.ToString("N2"),
                shippingFee = shippingFee.ToString("N2"),
                total = total.ToString("N2"),
                cartCount,
                amountToFreeShipping = Math.Max(0, 4000 - subtotal).ToString("N2"),
                freeShippingProgress = (int)Math.Min(100, (subtotal / 4000m) * 100)
            });
        }


        public IActionResult ChangePassword()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Auth");

            ViewData["Title"] = "Change Password";
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Auth");

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return RedirectToAction("Login", "Auth");

            // ── BCrypt verify current password ──
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.Password))
            {
                TempData["Error"] = "Current password is incorrect!";
                return View();
            }

            if (newPassword != confirmPassword)
            {
                TempData["Error"] = "New passwords do not match!";
                return View();
            }

            // ── Hash the new password ──
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _context.Users.Update(user);
            _context.SaveChanges();

            TempData["Success"] = "Password changed successfully!";
            return RedirectToAction("Profile");
        }

        public IActionResult ManageAddresses()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login", "Auth");

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return RedirectToAction("Login", "Auth");

            var addresses = _context.Addresses
                .Where(a => a.UserId == user.Id)
                .OrderByDescending(a => a.IsDefault)
                .ToList();

            return View(addresses);
        }

        [HttpPost]
        public IActionResult AddAddress(string label, string fullAddress, string city, string province, string zipCode, bool isDefault)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return RedirectToAction("Login", "Auth");

            // ── kung isDefault, i-remove ang default sa uban ──
            if (isDefault)
            {
                var existing = _context.Addresses.Where(a => a.UserId == user.Id);
                foreach (var a in existing) a.IsDefault = false;
            }

            var address = new Address
            {
                UserId = user.Id,
                Label = label,
                FullAddress = fullAddress,
                City = city,
                Province = province,
                ZipCode = zipCode,
                IsDefault = isDefault
            };

            _context.Addresses.Add(address);
            _context.SaveChanges();

            TempData["Success"] = "Address added successfully!";
            return RedirectToAction("ManageAddresses");
        }

        [HttpPost]
        public IActionResult DeleteAddress(int id)
        {
            var address = _context.Addresses.FirstOrDefault(a => a.Id == id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
                _context.SaveChanges();
                TempData["Success"] = "Address deleted!";
            }
            return RedirectToAction("ManageAddresses");
        }

        [HttpPost]
        public IActionResult SetDefaultAddress(int id)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return RedirectToAction("Login", "Auth");

            var allAddresses = _context.Addresses.Where(a => a.UserId == user.Id);
            foreach (var a in allAddresses) a.IsDefault = false;

            var selected = allAddresses.FirstOrDefault(a => a.Id == id);
            if (selected != null) selected.IsDefault = true;

            _context.SaveChanges();
            TempData["Success"] = "Default address updated!";
            return RedirectToAction("ManageAddresses");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult PlaceOrder(string fullName, string phone, string fullAddress,
            string city, string province, string zipCode,
            string gcashNumber, string gcashName, string paymentMethod = "GCash")
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return Json(new { success = false, redirect = "/Auth/Login" });

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return Json(new { success = false });

            var cartItems = _context.CartItems
                .Where(c => c.UserId == user.Id)
                .Select(c => new
                {
                    c.Id,
                    c.PlantId,
                    c.Plant.Name,
                    c.Plant.ImageUrl,
                    Price = c.Plant.DiscountPercent.HasValue
                        ? c.Plant.Price * (1 - c.Plant.DiscountPercent.Value / 100m)
                        : c.Plant.Price,
                    c.Quantity
                })
                .ToList();

            if (!cartItems.Any())
                return Json(new { success = false, message = "Cart is empty" });

            decimal subtotal = cartItems.Sum(c => c.Price * c.Quantity);
            decimal shippingFee = subtotal >= 4000 ? 0 : 150;
            decimal total = subtotal + shippingFee;

            // ── Create Order ──
            var order = new Order
            {
                UserId = user.Id,
                FullName = fullName,
                Phone = phone,
                FullAddress = fullAddress,
                City = city,
                Province = province,
                ZipCode = zipCode,
                PaymentMethod = paymentMethod,
                GCashNumber = gcashNumber,
                GCashName = gcashName,
                Status = paymentMethod == "COD" ? "ToShip" : "Pending",
                Subtotal = subtotal,
                ShippingFee = shippingFee,
                Total = total,
                OrderedAt = DateTime.Now,
                Items = cartItems.Select(c => new OrderItem
                {
                    PlantId = c.PlantId,
                    PlantName = c.Name,
                    PlantImageUrl = c.ImageUrl,
                    Price = c.Price,
                    Quantity = c.Quantity
                }).ToList()
            };

            _context.Orders.Add(order);

            // ── Clear Cart ──
            var cartRows = _context.CartItems.Where(c => c.UserId == user.Id);
            _context.CartItems.RemoveRange(cartRows);

            // ── Create Notification ──
            _context.Notifications.Add(new Notification
            {
                UserId = user.Id,
                Title = "Order Placed! 🌿",
                Message = paymentMethod == "COD"
                    ? $"Your order of {cartItems.Count} item(s) worth ₱{total:N2} has been placed! Pay ₱{total:N2} upon delivery."
                    : $"Your order of {cartItems.Count} item(s) worth ₱{total:N2} has been placed! We'll verify your GCash payment shortly.",
                Type = "Order",
                IsRead = false,
                CreatedAt = DateTime.Now
            });

            _context.SaveChanges();

            return Json(new { success = true, orderId = order.Id });
        }
        [HttpPost]
        [IgnoreAntiforgeryToken]
        public IActionResult CancelOrder(int orderId, string reason)
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return Json(new { success = false, redirect = "/Auth/Login" });

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return Json(new { success = false });

            var order = _context.Orders
                .FirstOrDefault(o => o.Id == orderId && o.UserId == user.Id);

            if (order == null)
                return Json(new { success = false, message = "Order not found" });

            if (order.Status != "Pending" && order.Status != "ToShip")
                return Json(new { success = false, message = "Order cannot be cancelled" });

            order.Status = "Cancelled";
            _context.Orders.Update(order);

            // ── Notification ──
            _context.Notifications.Add(new Notification
            {
                UserId = user.Id,
                Title = "Order Cancelled 😔",
                Message = $"Order #PT-{order.Id:D4} has been cancelled. Reason: {reason}.",
                Type = "Order",
                IsRead = false,
                CreatedAt = DateTime.Now
            });

            _context.SaveChanges();

            return Json(new { success = true });
        }
        [HttpGet]
        [IgnoreAntiforgeryToken]
        public IActionResult GetCartCount()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
                return Json(new { count = 0 });

            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
                return Json(new { count = 0 });

            var count = _context.CartItems
                .Where(c => c.UserId == user.Id)
                .Sum(c => (int?)c.Quantity) ?? 0;

            return Json(new { count });
        }
    }
}