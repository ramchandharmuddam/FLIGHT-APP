using Microsoft.AspNetCore.Mvc;

namespace Group3Flight.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Flights()
        {
            return View();
        }
    }
}
