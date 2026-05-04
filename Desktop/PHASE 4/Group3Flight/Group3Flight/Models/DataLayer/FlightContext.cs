using Group3Flight.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace Group3Flight.Models.DataLayer
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
            modelBuilder.ApplyConfiguration(new ConfigureFlight());
            modelBuilder.ApplyConfiguration(new ConfigureAirline());
        }
    }
}
