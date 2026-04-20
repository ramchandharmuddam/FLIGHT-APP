namespace Group3Flight.Models
{
    public class FlightDataSessions
    {
        private const string FlightReservationKey = "flightReservationskey";
        private const string ReservationCountKey = "reservationCountKey";
        private const string ActiveFromKey = "fromKey";
        private const string ActiveToKey = "toKey";
        private const string ActiveDepartureDate = "deptDateKey";
        private const string ActiveCabinType = "cabinTypeKey";

        private ISession session { get; set; }
        public FlightDataSessions(ISession session) => this.session = session;

        public void SetFlightReservations(List<FlightReservations> flightReservations)
        {
            session.SetObject(FlightReservationKey, flightReservations);
            session.SetInt32(ReservationCountKey, flightReservations.Count);
        }
        public List<FlightReservations> GetFlightReservations() =>
            session.GetObject<List<FlightReservations>>(FlightReservationKey) ?? new List<FlightReservations>();
        public int? GetFlightReservationsCount() => session.GetInt32(ReservationCountKey);

        public void SetActiveFrom(string activeFrom) =>
            session.SetString(ActiveFromKey, activeFrom);
        public string GetActiveFrom() =>
            session.GetString(ActiveFromKey) ?? string.Empty;

        public void SetActiveTo(string activeTo) =>
            session.SetString(ActiveToKey, activeTo);
        public string GetActiveTo() =>
            session.GetString(ActiveToKey) ?? string.Empty;
        public void SetActiveCabinType(string activeCabinType) =>
            session.SetString(ActiveCabinType, activeCabinType);
        public string GetActiveCabinType() =>
            session.GetString(ActiveCabinType) ?? string.Empty;
        public void SetActiveDepartureDate(string activeDeptDate) =>
            session.SetString(ActiveDepartureDate, activeDeptDate);
        public string GetActiveDepartureDate() =>
            session.GetString(ActiveDepartureDate) ?? string.Empty;
        public void SaveFilterCriteria(string From, string To, string DeptDate, string CabinType)
        {
            SetActiveFrom(From);
            SetActiveTo(To);
            SetActiveDepartureDate(DeptDate);
            SetActiveCabinType(CabinType);
        }
    }
}
