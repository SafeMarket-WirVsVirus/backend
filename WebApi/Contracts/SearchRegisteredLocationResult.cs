using System.Collections.Generic;

namespace ReservationSystem.Contracts
{
    public class SearchRegisteredLocationResult
    {
        public IEnumerable<LocationResult> Locations { get; set; }
    }
}