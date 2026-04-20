namespace Group3Flight.Models
{
    public class FlightDataCookies
    {
        private const string FlightReservationKey = "flightReservationskey";
        private const string Delimiter = "-";

        private IRequestCookieCollection requestCookies { get; set; } = null!;
        private IResponseCookies responseCookies { get; set; } = null!;

        public FlightDataCookies(IRequestCookieCollection request, IResponseCookies response)
        {
            requestCookies = request;
            responseCookies = response;
        }
        public void RemoveFlightReservationId(int id)
        {
            string[] ids = GetFlightReservationsIds();
            var updatedIds = ids.Where(rid => rid != id.ToString()).ToArray();
            SetFlightReservationsIds(updatedIds);
        }
        public void SetFlightReservationsIds(List<FlightReservations> myBookings)
        {
            var ids = myBookings.Select(r => r.FlightReservationsId.ToString()).ToList();
            SetFlightReservationsIds(ids);
        }
        public void SetFlightReservationsIds(IEnumerable<string> ids)
        {
            if (responseCookies == null)
                throw new InvalidOperationException("Response cookies are not initialized.");

            string idsString = string.Join(Delimiter, ids);
            CookieOptions options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(7),
                IsEssential = true
            };

            RemoveFlightReservationsIds();
            responseCookies.Append(FlightReservationKey, idsString, options);
        }
        public string[] GetFlightReservationsIds()
        {
            string cookie = requestCookies[FlightReservationKey] ?? String.Empty;
            if (string.IsNullOrEmpty(cookie))
                return Array.Empty<string>();
            else
                return cookie.Split(Delimiter);
        }

        public void RemoveFlightReservationsIds()
        {
            if (responseCookies == null)
                throw new InvalidOperationException("Response cookies are not initialized.");

            responseCookies.Delete(FlightReservationKey);
        }
    }
}
