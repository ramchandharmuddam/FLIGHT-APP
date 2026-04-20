namespace Group3Flight.Models
{
    public class AirlineViewModel
    {
        public string ActiveFromKey { get; set; } = "all";
        public string ActiveToKey { get; set; } = "all";
        public string ActiveDepartureDate { get; set; } = "all";
        public string ActiveCabinType { get; set; } = "all";
        public List<string> CabinTypes { get; set; } = new List<string>();
        public List<Flight> Flight { get; set; } = new List<Flight>();
        public List<FlightReservations> FlightReservation { get; set; } = new List<FlightReservations>();
        public List<Airline> Airline { get; set; } = new List<Airline>();
        public Flight Flights { get; set; } = new Flight();
        public FlightReservations FlightReservations { get; set; } = new FlightReservations();
        public Airline Airlines { get; set; } = new Airline();

        public string CheckActiveFrom(string d) =>
            d.ToLower() == ActiveFromKey.ToLower() ? "active" : "";
        public string CheckActiveTo(string d) =>
            d.ToLower() == ActiveToKey.ToLower() ? "active" : "";
        public string CheckActiveDepartureDate(string d) =>
            d.ToLower() == ActiveDepartureDate.ToLower() ? "active" : "";
        public string CheckActiveCabinType(string d) =>
            d.ToLower() == ActiveCabinType.ToLower() ? "active" : "";
    }
}
