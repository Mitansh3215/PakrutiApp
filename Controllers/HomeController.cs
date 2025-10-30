using Microsoft.AspNetCore.Mvc;
using PakrutiApp.Data; // or your actual namespace
using System.Linq;

namespace PakrutiApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            ViewBag.User = user;
            return View();
        }
    }
}
