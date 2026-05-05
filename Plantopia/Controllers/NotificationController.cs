using Microsoft.AspNetCore.Mvc;
using Plantopia.Data;
using Plantopia.Models;

namespace Plantopia.Controllers
{
    public class NotificationController : Controller
    {
        private readonly AppDbContext _context;

        public NotificationController(AppDbContext context)
        {
            _context = context;
        }

        // ── Get unread count (para sa badge) ──
        [HttpGet]
        public IActionResult GetUnreadCount()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return Json(new { count = 0 });

            var count = _context.Notifications
                .Count(n => n.UserId == user.Id && !n.IsRead);

            return Json(new { count });
        }

        // ── Get all notifications (para sa dropdown) ──
        [HttpGet]
        public IActionResult GetAll()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return Json(new List<object>());

            var notifications = _context.Notifications
                .Where(n => n.UserId == user.Id)
                .OrderByDescending(n => n.CreatedAt)
                .Take(10)
                .Select(n => new {
                    n.Id,
                    n.Title,
                    n.Message,
                    n.Type,
                    n.IsRead,
                    CreatedAt = n.CreatedAt.ToString("MMM dd, h:mm tt")
                })
                .ToList();

            return Json(notifications);
        }

        // ── Mark one as read ──
        [HttpPost]
        public IActionResult MarkAsRead(int id)
        {
            var notif = _context.Notifications.FirstOrDefault(n => n.Id == id);
            if (notif != null)
            {
                notif.IsRead = true;
                _context.SaveChanges();
            }
            return Json(new { success = true });
        }

        // ── Mark all as read ──
        [HttpPost]
        public IActionResult MarkAllRead()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) return Json(new { success = false });

            var unread = _context.Notifications
                .Where(n => n.UserId == user.Id && !n.IsRead)
                .ToList();

            foreach (var n in unread) n.IsRead = true;
            _context.SaveChanges();

            return Json(new { success = true });
        }

        // ── Helper: Create notification (tawgon ni sa ibang controllers) ──
        public static void Create(AppDbContext context, int userId,
            string title, string message, string type = "Order")
        {
            context.Notifications.Add(new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type
            });
            context.SaveChanges();
        }
    }
}