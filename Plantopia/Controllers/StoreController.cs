using Microsoft.AspNetCore.Mvc;

namespace Plantopia.Controllers
{
    public class StoreController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Plants()
        {
            return View();
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