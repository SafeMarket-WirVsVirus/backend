using System.Collections.Generic;

namespace ReservationSystem.Contracts
{
    public class GetReservationsResult
    {
        public List<ReservationContract> Reservations { get; set; }
    }
}