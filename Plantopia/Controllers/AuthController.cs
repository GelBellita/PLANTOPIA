using Microsoft.AspNetCore.Mvc;
using Plantopia.Models;

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
    }
}