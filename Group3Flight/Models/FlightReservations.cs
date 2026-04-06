using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Group3Flight.Models
{
    public class FlightReservations
    {
        public int FlightReservationsId { get; set; }
        public int FlightId { get; set; }
        [ValidateNever]
        public Flight Flight { get; set; } = null!;
    }
}
