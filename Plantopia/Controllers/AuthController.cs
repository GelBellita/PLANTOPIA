using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Plantopia.Data;
using Plantopia.Models;
using System.Security.Claims;

namespace Plantopia.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        // ── Dependency Injection
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // ── HttpGet 
        [HttpGet("login")]
        public IActionResult Login() => View(new LoginModel());

        // ── HttpPost 
        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            // ── ModelState.IsValid like
            if (!ModelState.IsValid) return View(model);

            // ── LINQ query sa database 
            var user = _context.Users
    .FirstOrDefault(u => u.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            // ── Store session
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.FullName);

            return RedirectToAction("Index", "Store");
        }

        // ── HttpGet
        [HttpGet("register")]
        public IActionResult Register() => View(new RegisterModel());

        // ── HttpPost 
        [HttpPost("register")]
        public IActionResult Register(RegisterModel model)
        {
            // ── ModelState.IsValid 
            if (!ModelState.IsValid) return View(model);

            // ── Check if email already exists ──
            var existing = _context.Users
                .FirstOrDefault(u => u.Email == model.Email);

            if (existing != null)
            {
                ModelState.AddModelError("Email", "Email already registered.");
                return View(model);
            }


            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Role = "Customer"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["Success"] = $"Welcome, {model.FullName}! Please log in.";
            return RedirectToAction("Login");
        }

        // ── Logout ──
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}