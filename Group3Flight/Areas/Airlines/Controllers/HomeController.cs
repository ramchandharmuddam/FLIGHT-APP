using Group3Flight.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group3Flight.Areas.Airlines.Controllers
{
    [Area("Airlines")]
    public class HomeController : Controller
    {
        private FlightContext context { get; set; }

        public HomeController(FlightContext ctx) => context = ctx;

        public IActionResult Index()
        {
            var flights = context.Flight
                .Include(r => r.Airline)
                .OrderBy(m => m.FlightCode).ToList();
            return View(flights);
        }
    }
}
