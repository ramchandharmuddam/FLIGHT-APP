using Microsoft.EntityFrameworkCore;

namespace Group3Flight.Models
{
    public class FlightContext : DbContext
    {
        public FlightContext(DbContextOptions<FlightContext> options)
            : base(options) { }

        public DbSet<Airline> Airline { get; set; } = null!;
        public DbSet<Flight> Flight { get; set; } = null!;
        public DbSet<FlightReservations> FlightReservations { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Airline>().HasData(
                new Airline { AirlineId = 11, Name = "Spirit Airlines", ImageName = "SpiritAirlines.png" },
                new Airline { AirlineId = 12, Name = "Frontier Airlines", ImageName = "FrontierAirlines.png" },
                new Airline { AirlineId = 13, Name = "Hawaiian Airlines", ImageName = "HawaiianAirlines.png" },
                new Airline { AirlineId = 14, Name = "Allegiant Air", ImageName = "AllegiantAir.png" },
                new Airline { AirlineId = 15, Name = "Sun Country Airlines", ImageName = "SunCountry.png" },
                new Airline { AirlineId = 16, Name = "Silver Airways", ImageName = "SilverAirways.png" }
            );

            modelBuilder.Entity<Flight>().HasData(
                new Flight
                {
                    FlightId = 1,
                    FlightCode = "NK601",
                    From = "Miami",
                    To = "Houston",
                    Date = new DateTime(2026, 6, 5),
                    DepartureTime = new TimeSpan(7, 15, 0),
                    ArrivalTime = new TimeSpan(9, 30, 0),
                    CabinType = "Economy",
                    Emission = 200.3,
                    AircraftType = "Airbus A321",
                    Price = 150.00M,
                    AirlineId = 11,
                },
                new Flight
                {
                    FlightId = 2,
                    FlightCode = "FA123",
                    From = "Las Vegas",
                    To = "Orlando",
                    Date = new DateTime(2026, 7, 8),
                    DepartureTime = new TimeSpan(12, 45, 0),
                    ArrivalTime = new TimeSpan(20, 10, 0),
                    CabinType = "Basic Economy",
                    Emission = 300.5,
                    AircraftType = "Airbus A320neo",
                    Price = 275.00M,
                    AirlineId = 12,
                },
                new Flight
                {
                    FlightId = 3,
                    FlightCode = "HA789",
                    From = "Honolulu",
                    To = "San Diego",
                    Date = new DateTime(2026, 8, 10),
                    DepartureTime = new TimeSpan(16, 0, 0),
                    ArrivalTime = new TimeSpan(23, 30, 0),
                    CabinType = "Business",
                    Emission = 410.8,
                    AircraftType = "Airbus A330",
                    Price = 680.00M,
                    AirlineId = 13,
                },
                new Flight
                {
                    FlightId = 4,
                    FlightCode = "GN450",
                    From = "Nashville",
                    To = "Phoenix",
                    Date = new DateTime(2026, 9, 14),
                    DepartureTime = new TimeSpan(10, 20, 0),
                    ArrivalTime = new TimeSpan(12, 55, 0),
                    CabinType = "Economy",
                    Emission = 220.4,
                    AircraftType = "Boeing 737 MAX",
                    Price = 195.00M,
                    AirlineId = 14,
                },
                new Flight
                {
                    FlightId = 5,
                    FlightCode = "SY880",
                    From = "Minneapolis",
                    To = "Cancun",
                    Date = new DateTime(2026, 10, 18),
                    DepartureTime = new TimeSpan(6, 0, 0),
                    ArrivalTime = new TimeSpan(10, 45, 0),
                    CabinType = "Economy Plus",
                    Emission = 350.9,
                    AircraftType = "Boeing 737-800",
                    Price = 520.00M,
                    AirlineId = 15,
                },
                new Flight
                {
                    FlightId = 6,
                    FlightCode = "TR222",
                    From = "Tampa",
                    To = "Key West",
                    Date = new DateTime(2026, 11, 20),
                    DepartureTime = new TimeSpan(9, 10, 0),
                    ArrivalTime = new TimeSpan(10, 25, 0),
                    CabinType = "Economy",
                    Emission = 90.6,
                    AircraftType = "ATR 72",
                    Price = 130.00M,
                    AirlineId = 16,
                }
            );
        }
    }
}
