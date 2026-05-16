using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
