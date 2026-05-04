using Group3Flight.Models;
using Group3Flight.Models.DomainModels;
using Group3Flight.Models.Validations;
using Group3Flight.Models.DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Group3Flight.Areas.Airlines.Controllers
{
    [Area("Airlines")]
    public class FlightsController : Controller
    {
        private IRepository<Flight> flightRepo;
        private IRepository<Airline> airlineRepo;
        private IRepository<FlightReservations> flightResRepo;

        public FlightsController(
            IRepository<Flight> FlightRepo,
            IRepository<Airline> AirRepo,IRepository<FlightReservations> FlightResRepo)
        {
            flightRepo = FlightRepo;
            airlineRepo = AirRepo;
            flightResRepo = FlightResRepo;

        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Action = "Add";
            LoadAirlines();

            return View("Edit", new Flight());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Action = "Edit";
            ViewBag.Disable = "";

            LoadAirlines();

            var flight = flightRepo.Get(id);

            return View(flight);
        }

        [HttpPost]
        public IActionResult Edit(Flight flight)
        {
            if (TempData["okFlight"] == null)
            {
                string msg = Check.IsFlightAvailable(
                    GetDbContext(),
                    flight.Date,
                    flight.FlightCode);

                if (!string.IsNullOrEmpty(msg))
                {
                    ModelState.AddModelError(
                        nameof(flight.FlightCode),
                        msg);
                }
            }

            if (ModelState.IsValid)
            {
                if (flight.FlightId == 0)
                {
                    flightRepo.Insert(flight);

                    TempData["Message"] =
                        $"{flight.FlightCode} Added Successfully";
                }
                else
                {
                    flightRepo.Update(flight);

                    TempData["Message"] =
                        $"{flight.FlightCode} updated successfully.";
                }

                flightRepo.Save();

                return RedirectToAction("Index", "Home");
            }

            LoadAirlines();

            ViewBag.Action =
                (flight.FlightId == 0) ? "Add" : "Edit";

            return View(flight);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var flight = flightRepo.Get(id);

            return View(flight);
        }

        [HttpPost]
        public IActionResult Delete(Flight flight)
        {
            // CHECK IF FLIGHT HAS RESERVATIONS
            bool hasReservations = flightResRepo
                .List(new QueryOptions<FlightReservations>())
                .Any(b => b.FlightId == flight.FlightId);

            if (hasReservations)
            {
                TempData["Message"] =
                    $"Cannot delete this flight {flight.FlightCode} because this flight has existing reservations.";

                return RedirectToAction("Index", "Home");
            }

            flightRepo.Delete(flight);
            flightRepo.Save();

            TempData["Message"] =
                $"{flight.FlightCode} Deleted Successfully";

            return RedirectToAction("Index", "Home");
        }

        // ---------------- EXTRA PAGES ----------------
        public IActionResult Manage()
        {
            return View();
        }

        public IActionResult Regulation()
        {
            return View();
        }

        private void LoadAirlines()
        {
            var options = new QueryOptions<Airline>();

            var airlines = airlineRepo.List(options)
                .OrderBy(a => a.AirlineId)
                .Select(a => new SelectListItem
                {
                    Value = a.AirlineId.ToString(),
                    Text = a.Name
                })
                .ToList();

            ViewBag.Airlines = airlines;
        }

        // TEMP helper only because your validation uses DbContext
        private FlightContext GetDbContext()
        {
            return (FlightContext)
                HttpContext.RequestServices
                .GetService(typeof(FlightContext));
        }
    }
}