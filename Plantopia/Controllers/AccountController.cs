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
    }
}