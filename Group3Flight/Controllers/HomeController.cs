using Group3Flight.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group3Flight.Controllers
{
    public class HomeController : Controller
    {
        private FlightContext _context;
        public HomeController(FlightContext context)
        {
            _context = context;
        }

        // GET: Home/Index
        [HttpGet]
        public ViewResult Index(AirlineViewModel model)
        {
            var session = new FlightDataSessions(HttpContext.Session);
            var cookies = new FlightDataCookies(Request.Cookies, Response.Cookies);

            // Restore saved filters if model comes in empty
            if (string.IsNullOrEmpty(model.ActiveFromKey))
                model.ActiveFromKey = session.GetActiveFrom();

            if (string.IsNullOrEmpty(model.ActiveToKey))
                model.ActiveToKey = session.GetActiveTo();

            if (string.IsNullOrEmpty(model.ActiveDepartureDate))
                model.ActiveDepartureDate = session.GetActiveDepartureDate();

            if (string.IsNullOrEmpty(model.ActiveCabinType))
                model.ActiveCabinType = session.GetActiveCabinType();

            // Optional safe defaults
            model.ActiveFromKey ??= "";
            model.ActiveToKey ??= "";
            model.ActiveCabinType ??= "All";

            // Fix for date parsing bug
            string datePart = "all";
            if (!string.IsNullOrEmpty(model.ActiveDepartureDate) &&
                DateTime.TryParse(model.ActiveDepartureDate, out DateTime activeDepartureDate))
            {
                datePart = activeDepartureDate.ToString("yyyy-MM-dd");
            }

            var filterList = new Filter(
                $"{model.ActiveFromKey}-{model.ActiveToKey}-{datePart}-{model.ActiveCabinType}"
            );
            var filters = new Filter(filterList.FilterString);

            // Save current filters in session
            session.SaveFilterCriteria(
                model.ActiveFromKey,
                model.ActiveToKey,
                model.ActiveDepartureDate,
                model.ActiveCabinType
            );

            if (session.GetFlightReservationsCount() == 0)
            {
                var ids = cookies.GetFlightReservationsIds();
                if (ids.Length > 0)
                {
                    var reservations = _context.FlightReservations
                        .Include(r => r.Flight)
                        .ThenInclude(r => r.Airline)
                        .Where(r => ids.Contains(r.FlightReservationsId.ToString()))
                        .ToList();

                    session.SetFlightReservations(reservations);
                }
            }

            IQueryable<Flight> query = _context.Flight
                .Include(r => r.Airline)
                .OrderBy(r => r.FlightCode);

            if (filters.HasFromKey && !string.IsNullOrEmpty(model.ActiveFromKey))
                query = query.Where(r => r.From.ToString() == model.ActiveFromKey);

            if (filters.HasToKey && !string.IsNullOrEmpty(model.ActiveToKey))
                query = query.Where(r => r.To.ToString() == model.ActiveToKey);

            if (!string.IsNullOrEmpty(model.ActiveDepartureDate) &&
                model.ActiveDepartureDate.ToLower() != "all")
            {
                DateTime selectedDate = DateTime.Parse(model.ActiveDepartureDate);
                query = query.Where(r => r.Date.Date == selectedDate.Date);
            }

            if (!string.IsNullOrEmpty(model.ActiveCabinType) &&
                model.ActiveCabinType.ToLower() != "all")
            {
                query = query.Where(r => r.CabinType == model.ActiveCabinType);
            }

            // Keep your current pattern here to avoid breaking the view
              model.CabinTypes = _context.Flight
                .Select(f => f.CabinType)
                .Distinct()
                .ToList();

            model.Flight = query.ToList();
            foreach (var f in model.Flight)
{
    if (f.AircraftType == "ATR 72" || f.AircraftType == "Airbus A320neo")
    {
        f.AircraftType = "Airbus A320 Family";
    }
}
_context.SaveChanges();
            return View(model);
        }

        // POST: Home/Index
        // PRG fix: save filters, then redirect to GET Index
        [HttpPost]
        [ActionName("Index")]
        public IActionResult IndexPost(AirlineViewModel model)
        {
            var session = new FlightDataSessions(HttpContext.Session);

            session.SaveFilterCriteria(
                model.ActiveFromKey,
                model.ActiveToKey,
                model.ActiveDepartureDate,
                model.ActiveCabinType
            );

            return RedirectToAction("Index", new
            {
                ActiveFromKey = model.ActiveFromKey,
                ActiveToKey = model.ActiveToKey,
                ActiveDepartureDate = model.ActiveDepartureDate,
                ActiveCabinType = model.ActiveCabinType
            });
        }

        [HttpGet]
        public IActionResult FlightReserve(int id)
        {
            var session = new FlightDataSessions(HttpContext.Session);
            var cookies = new FlightDataCookies(Request.Cookies, Response.Cookies);

            var flightReservations = new FlightReservations
            {
                FlightId = id,
            };

            _context.FlightReservations.Add(flightReservations);
            _context.SaveChanges();

            var flightReservations1 = session.GetFlightReservations();
            flightReservations1.Add(flightReservations);
            session.SetFlightReservations(flightReservations1);
            cookies.SetFlightReservationsIds(flightReservations1);

            TempData["Message"] = "Booking successful! Your ticket has been confirmed.";

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
            var fRIds = cookies.GetFlightReservationsIds();

            var flightReservations = _context.FlightReservations
                .Include(r => r.Flight)
                .Where(r => fRIds.Contains(r.FlightReservationsId.ToString()))
                .ToList();

            var model = new AirlineViewModel
            {
                FlightReservation = flightReservations,
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

            var flightReservations = session.GetFlightReservations();

            if (flightReservations != null && flightReservations.Any())
            {
                var ids = flightReservations.Select(b => b.FlightReservationsId).ToList();

                var flightReservations1 = _context.FlightReservations
                    .Where(b => ids.Contains(b.FlightReservationsId))
                    .ToList();

                _context.FlightReservations.RemoveRange(flightReservations1);
                _context.SaveChanges();

                session.SetFlightReservations(new List<FlightReservations>());

                var cookies = new FlightDataCookies(Request.Cookies, Response.Cookies);
                foreach (var id in ids)
                {
                    cookies.RemoveFlightReservationId(id);
                }
            }

            TempData["Message"] = "All bookings cancelled successfully!";

            // PRG fix: redirect to Home with retained filters
            return RedirectToAction("Index", new
            {
                ActiveFromKey = session.GetActiveFrom(),
                ActiveToKey = session.GetActiveTo(),
                ActiveDepartureDate = session.GetActiveDepartureDate(),
                ActiveCabinType = session.GetActiveCabinType()
            });
        }

        [HttpPost]
        public IActionResult CancelReservation(int id)
        {
            var session = new FlightDataSessions(HttpContext.Session);

            var flightReservations1 = _context.FlightReservations.Find(id);
            if (flightReservations1 != null)
            {
                _context.FlightReservations.Remove(flightReservations1);
                _context.SaveChanges();
            }

            var flightReservations = session.GetFlightReservations();
            var flightReservations2 = flightReservations.FirstOrDefault(r => r.FlightReservationsId == id);
            if (flightReservations2 != null)
            {
                flightReservations.Remove(flightReservations2);
                session.SetFlightReservations(flightReservations);
            }

            var cookies = new FlightDataCookies(Request.Cookies, Response.Cookies);
            cookies.RemoveFlightReservationId(id);

            TempData["Message"] = "Ticket cancelled successfully!";

            // PRG fix: redirect to Home with retained filters
            return RedirectToAction("Index", new
            {
                ActiveFromKey = session.GetActiveFrom(),
                ActiveToKey = session.GetActiveTo(),
                ActiveDepartureDate = session.GetActiveDepartureDate(),
                ActiveCabinType = session.GetActiveCabinType()
            });
        }

        public IActionResult Details(int id)
        {
            var flight = _context.Flight
                .Include(r => r.Airline)
                .FirstOrDefault(r => r.FlightId == id);

            if (flight == null)
                return NotFound();

            var session = new FlightDataSessions(HttpContext.Session);

            var airlineViewModel = new AirlineViewModel
            {
                Flights = flight,
                ActiveFromKey = session.GetActiveFrom(),
                ActiveToKey = session.GetActiveTo(),
                ActiveDepartureDate = session.GetActiveDepartureDate(),
                ActiveCabinType = session.GetActiveCabinType()
            };

            return View(airlineViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}