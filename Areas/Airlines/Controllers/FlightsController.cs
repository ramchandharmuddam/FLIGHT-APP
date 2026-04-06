using Microsoft.AspNetCore.Mvc;

namespace Group3Flight.Areas.Airlines.Controllers
{
    [Area("Airlines")]
    public class FlightsController : Controller
    {
        public IActionResult Manage()
        {
            return View();
        }
        public IActionResult Regulation()
        {
            return View();
        }
    }
}
