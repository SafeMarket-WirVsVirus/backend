using System.Collections.Generic;
using WebApi.Services;

namespace ReservationSystem.Contracts
{
    public class SearchLocationResult
    {
        public IEnumerable<PlacesTextsearchResponse> Locations { get; set; }
    }
}