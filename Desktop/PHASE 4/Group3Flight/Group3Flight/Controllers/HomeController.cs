using Group3Flight.Models;
using Group3Flight.Models.DomainModels;
using Group3Flight.Models.ViewModels;
using Group3Flight.Models.ExtensionMethods;
using Group3Flight.Models.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Group3Flight.Controllers
{
    public class HomeController : Controller
    {
        private IRepository<Flight> flightRepo;
        private IRepository<FlightReservations> reservationRepo;

        public HomeController(
            IRepository<Flight> repo,
            IRepository<FlightReservations> reservationRepo)
        {
            flightRepo = repo;
            this.reservationRepo = reservationRepo;
        }

        public ViewResult Index(AirlineViewModel model)
        {
            var filterList = new Filter(
                $"{model.ActiveFromKey}-{model.ActiveToKey}-" +
                $"{DateTime.TryParse(model.ActiveDepartureDate, out DateTime activeDepartureDate)}-" +
                $"{model.ActiveCabinType}");

            var filters = new Filter(filterList.FilterString);

            var session = new FlightDataSessions(HttpContext.Session);
            var cookies = new FlightDataCookies(Request.Cookies, Response.Cookies);

            // Save current filter values
            session.SaveFilterCriteria(
                model.ActiveFromKey,
                model.ActiveToKey,
                model.ActiveDepartureDate,
                model.ActiveCabinType
            );

            // Restore selected flights from cookies when session is empty
            var selectedFlights = session.GetFlightReservations();

            if (selectedFlights == null || !selectedFlights.Any())
            {
                var ids = cookies.GetFlightReservationsIds();

                if (ids.Length > 0)
                {
                    selectedFlights = ids.Select(id => new FlightReservations
                    {
                        FlightReservationsId = int.Parse(id),
                        FlightId = int.Parse(id)
                    }).ToList();

                    session.SetFlightReservations(selectedFlights);
                }
            }

            // Load flights with airline
            var options = new QueryOptions<Flight>
            {
                Includes = "Airline"
            };

            var flights = flightRepo.List(options)
                .OrderBy(f => f.FlightCode)
                .ToList();

            // Apply filters
            if (filters.HasFromKey)
            {
                flights = flights
                    .Where(f => f.From == model.ActiveFromKey)
                    .ToList();
            }

            if (filters.HasToKey)
            {
                flights = flights
                    .Where(f => f.To == model.ActiveToKey)
                    .ToList();
            }

            if (!string.IsNullOrEmpty(model.ActiveDepartureDate) &&
                model.ActiveDepartureDate.ToLower() != "all")
            {
                DateTime selectedDate = DateTime.Parse(model.ActiveDepartureDate);

                flights = flights
                    .Where(f => f.Date.Date == selectedDate.Date)
                    .ToList();
            }

            // Cabin dropdown values
            model.CabinTypes = flightRepo.List(new QueryOptions<Flight>())
                .Select(f => f.CabinType)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            model.Flight = flights;

            return View(model);
        }

        [HttpGet]
        public IActionResult FlightReserve(int id)
        {
            var session = new FlightDataSessions(HttpContext.Session);
            var cookies = new FlightDataCookies(Request.Cookies, Response.Cookies);

            var selected = session.GetFlightReservations();

            if (!selected.Any(r => r.FlightId == id))
            {
                selected.Add(new FlightReservations
                {
                    FlightReservationsId = id,
                    FlightId = id
                });

                session.SetFlightReservations(selected);
                cookies.SetFlightReservationsIds(selected);
            }

            TempData["Message"] =
                "Flight selected successfully!";

            return RedirectToAction("Index", new
            {
                ActiveFromKey = session.GetActiveFrom(),
                ActiveToKey = session.GetActiveTo(),
                ActiveDepartureDate = session.GetActiveDepartureDate(),
                ActiveCabinType = session.GetActiveCabinType()
            });
        }

        public IActionResult FlightReservations()
        {
            var session = new FlightDataSessions(HttpContext.Session);
            var cookies = new FlightDataCookies(Request.Cookies, Response.Cookies);

            var selected = session.GetFlightReservations();

            // If session empty, restore from cookies
            if (selected == null || !selected.Any())
            {
                var ids = cookies.GetFlightReservationsIds();

                if (ids.Length > 0)
                {
                    selected = ids.Select(id => new FlightReservations
                    {
                        FlightReservationsId = int.Parse(id),
                        FlightId = int.Parse(id)
                    }).ToList();

                    session.SetFlightReservations(selected);
                }
                else
                {
                    selected = new List<FlightReservations>();
                }
            }

            // IMPORTANT: Load Flight object for each selected reservation
            foreach (var item in selected)
            {
                var options = new QueryOptions<Flight>
                {
                    Includes = "Airline",
                    Where = f => f.FlightId == item.FlightId
                };

                item.Flight = flightRepo.Get(options);
            }

            var model = new AirlineViewModel
            {
                FlightReservation = selected,
                ActiveFromKey = session.GetActiveFrom(),
                ActiveToKey = session.GetActiveTo(),
                ActiveDepartureDate = session.GetActiveDepartureDate(),
                ActiveCabinType = session.GetActiveCabinType()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CancelAllReservations()
        {
            var session = new FlightDataSessions(HttpContext.Session);
            var cookies = new FlightDataCookies(Request.Cookies, Response.Cookies);

            var selected = session.GetFlightReservations();

            if (selected != null && selected.Any())
            {
                var ids = selected
                    .Select(r => r.FlightReservationsId)
                    .ToList();

                session.SetFlightReservations(
                    new List<FlightReservations>());

                foreach (var id in ids)
                {
                    cookies.RemoveFlightReservationId(id);
                }
            }

            TempData["Message"] =
                "All selected flights cancelled successfully!";

            return RedirectToAction("FlightReservations");
        }

        [HttpPost]
        public IActionResult CancelReservation(int id)
        {
            var session = new FlightDataSessions(HttpContext.Session);
            var cookies = new FlightDataCookies(Request.Cookies, Response.Cookies);

            var selected = session.GetFlightReservations();

            var item = selected
                .FirstOrDefault(r => r.FlightReservationsId == id);

            if (item != null)
            {
                selected.Remove(item);
                session.SetFlightReservations(selected);
            }

            cookies.RemoveFlightReservationId(id);

            TempData["Message"] =
                "Selected flight cancelled successfully!";

            return RedirectToAction("FlightReservations");
        }

        [HttpPost]
        public IActionResult ReserveAllFlights()
        {
            var session = new FlightDataSessions(HttpContext.Session);
            var cookies = new FlightDataCookies(Request.Cookies, Response.Cookies);

            var selectedFlights = session.GetFlightReservations();

            if (selectedFlights == null || !selectedFlights.Any())
            {
                TempData["Error"] = "No selected flights found.";
                return RedirectToAction("FlightReservations");
            }

            foreach (var item in selectedFlights)
            {
                bool exists = reservationRepo
                    .List(new QueryOptions<FlightReservations>())
                    .Any(r => r.FlightId == item.FlightId);

                if (!exists)
                {
                    reservationRepo.Insert(new FlightReservations
                    {
                        FlightId = item.FlightId
                    });
                }
            }

            reservationRepo.Save();

            session.SetFlightReservations(
                new List<FlightReservations>());

            cookies.RemoveFlightReservationsIds();

            TempData["Message"] =
                "All selected flights reserved successfully!";

            return RedirectToAction("FlightReservations");
        }

        public IActionResult Details(int id)
        {
            var options = new QueryOptions<Flight>();
            options.Includes = "Airline";
            options.Where = f => f.FlightId == id;

            var flight = flightRepo.Get(options);

            if (flight == null)
                return NotFound();

            var session = new FlightDataSessions(HttpContext.Session);

            var model = new AirlineViewModel
            {
                Flights = flight,
                ActiveFromKey = session.GetActiveFrom(),
                ActiveToKey = session.GetActiveTo(),
                ActiveDepartureDate = session.GetActiveDepartureDate(),
                ActiveCabinType = session.GetActiveCabinType()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}