using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Plantopia.Models;
using System.Security.Claims;

namespace Plantopia.Controllers
{

    [Route("auth")]
    public class AuthController : Controller
    {

        [HttpPost("login")]
        public IActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View(model);  

            if (model.Email == "demo@plantopia.ph" && model.Password == "plant123")
            {
                HttpContext.Session.SetString("UserEmail", model.Email);
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

       
        [HttpGet("login")]
        public IActionResult Login() => View(new LoginModel());

       
        [HttpGet("register")]
        public IActionResult Register() => View(new RegisterModel());

        
        [HttpPost("register")]
        public IActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid) return View(model);  

            TempData["Success"] = $"Welcome, {model.FullName}! Please log in.";
            return RedirectToAction("Login");
        }

        
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> DemoLogin()
        {
            // Hardcoded test user — no DB check
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, "Demo User"),
        new Claim(ClaimTypes.Email, "demo@plantopia.ph"),
        new Claim(ClaimTypes.Role, "Customer") // adjust to your role
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            return RedirectToAction("Index", "Store");
        }
    }
}