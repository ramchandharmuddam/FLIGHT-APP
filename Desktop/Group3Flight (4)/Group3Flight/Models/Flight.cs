using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Group3Flight.Models
{
    public class Flight
    {
        public int FlightId { get; set; }

        [Required(ErrorMessage = "Please enter a FlightCode.")]
        [RegularExpression(@"^[A-Za-z]{2}[0-9]{1,4}$", ErrorMessage = "FlightCode must start with 2 letters followed by 1-4 digits.")]
        public string FlightCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a From.")]
        [RegularExpression(@"^[A-Za-z]+$",ErrorMessage = "From must contain letters only.")]
        [StringLength(50, ErrorMessage = "From cannot exceed 50 characters.")]
        public string From { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a To.")]
        [RegularExpression(@"^[A-Za-z]+$",ErrorMessage = "To must contain letters only.")]
        [StringLength(50, ErrorMessage = "To cannot exceed 50 characters.")]
        public string To { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a Date.")]
        [ValidFutureDate(3, ErrorMessage = "The date must be valid, later than today, and no more than three years in the future.")]
        [Remote(action: "CheckFlightDate", controller: "Validation", areaName: "",
            AdditionalFields = nameof(FlightCode),
            ErrorMessage = "A flight for this date already exists.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please enter a DepartureTime.")]
        public TimeSpan DepartureTime { get; set; }

        [Required(ErrorMessage = "Please enter a ArrivalTime.")]
        public TimeSpan ArrivalTime { get; set; }

        [Required(ErrorMessage = "Please enter a CabinType.")]
        public string CabinType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a Emission.")]
        [Range(0, 5000, ErrorMessage = "Emission cannot exceed 5000 kg CO2e.")]
        public double Emission { get; set; }

        [Required(ErrorMessage = "Please enter a AircraftType.")]
        public string AircraftType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a Price.")]
        [Range(0, 50000, ErrorMessage = "Price must be between 0 and 50,000 USD.")]
        public decimal Price { get; set; }
        public int AirlineId { get; set; }
        [ValidateNever]
        public Airline Airline { get; set; } = null!;
    }
}
