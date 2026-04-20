using Group3Flight.Models;
using Microsoft.AspNetCore.Mvc;

namespace Group3Flight.Controllers
{
    public class ValidationController : Controller
    {
        private FlightContext context;
        public ValidationController(FlightContext ctx) => context = ctx;

        public JsonResult CheckFlightDate(string flightCode, DateTime date)
        {
            string msg = Check.IsFlightAvailable(context, date, flightCode);
            if (string.IsNullOrEmpty(msg))
            {
                TempData["okFlight"] = true;
                return Json(true);
            }
            else return Json(msg);
        }
    }
}
