using Group3Flight.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Group3Flight.Areas.Airlines.Controllers
{
    [Area("Airlines")]
    public class FlightsController : Controller
    {
        private FlightContext context { get; set; }

        public FlightsController(FlightContext ctx) => context = ctx;

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            var airlines = context.Airline
                .OrderBy(m => m.AirlineId).ToList();
            ViewBag.Airlines = airlines
                .Select(l => new SelectListItem
                {
                    Value = l.AirlineId.ToString(),
                    Text = l.Name
                })
                .ToList();
            return View("Edit", new Flight());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            ViewBag.Disable = "";
            var airlines = context.Airline
                .OrderBy(m => m.AirlineId).ToList();
            ViewBag.Airlines = airlines
                .Select(l => new SelectListItem
                {
                    Value = l.AirlineId.ToString(),
                    Text = l.Name
                })
                .ToList();
            var flight = context.Flight.Find(id);
            return View(flight);
        }

        [HttpPost]
        public IActionResult Edit(Flight flight)
        {
            if (TempData["okFlight"] == null)
            {
                string msg = Check.IsFlightAvailable(context, flight.Date, flight.FlightCode);
                if (!String.IsNullOrEmpty(msg))
                {
                    ModelState.AddModelError(nameof(flight.FlightCode), msg);
                }
            }
            if (ModelState.IsValid)
            {
                if (flight.FlightId == 0)
                {
                    context.Flight.Add(flight);
                    TempData["Message"] = $"{flight.FlightCode} Added Successfully";
                }
                else
                {
                    context.Flight.Update(flight);
                    TempData["Message"] = $"{flight.FlightCode} updated successfully.";
                }
                context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var airlines = context.Airline
                .OrderBy(m => m.AirlineId).ToList();
                ViewBag.Airlines = airlines
                .Select(l => new SelectListItem
                {
                    Value = l.AirlineId.ToString(),
                    Text = l.Name
                })
                .ToList();
                ViewBag.Action = (flight.FlightId == 0) ? "Add" : "Edit";
                return View(flight);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var flight = context.Flight.Find(id);
            return View(flight);
        }

        [HttpPost]
        public IActionResult Delete(Flight flight)
        {
            context.Flight.Remove(flight);
            context.SaveChanges();
            TempData["Message"] = $"{flight.FlightCode} Deleted Successfully";
            return RedirectToAction("Index", "Home");
        }
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
