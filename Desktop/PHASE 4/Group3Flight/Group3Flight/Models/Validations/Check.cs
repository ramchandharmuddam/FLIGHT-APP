using Group3Flight.Models.DataLayer;

namespace Group3Flight.Models.Validations
{
    public static class Check
    {
        public static string IsFlightAvailable(FlightContext ctx, DateTime date, string flightCode)
        {
            string msg = string.Empty;
            if (!string.IsNullOrWhiteSpace(flightCode) && date != DateTime.MinValue)
            {
                var flight = ctx.Flight.FirstOrDefault(f => f.FlightCode == flightCode && f.Date == date);
                if (flight != null)
                    msg = $"Flight {flightCode} already exists for this date.";
            }
            return msg;
        }
    }
}
