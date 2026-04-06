using Microsoft.AspNetCore.Mvc;

namespace Group3Flight.Areas.Airlines.Controllers
{
    [Area("Airlines")]
    public class HomeController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
