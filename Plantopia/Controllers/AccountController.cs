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

            // ── Update fields ──
            user.FullName = model.FullName;
            user.Phone = model.Phone;
            user.Location = model.Location;

            // ── SaveChanges 
            _context.Users.Update(user);
            _context.SaveChanges();

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
            ViewData["Title"] = "My Purchase";
            return View();
        }

        public IActionResult Wishlist()
        {
            ViewData["Title"] = "Wishlist";
            return View();
        }

        public IActionResult Messages()
        {
            ViewData["Title"] = "Messages";
            return View();
        }

        public IActionResult Cart()
        {
            ViewData["Title"] = "My Cart";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}