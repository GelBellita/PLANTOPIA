using Microsoft.AspNetCore.Mvc;

namespace Plantopia.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Profile()
        {
            ViewData["Title"] = "My Profile";
            return View();
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
            return RedirectToAction("Index", "Store");
        }
    }
}
