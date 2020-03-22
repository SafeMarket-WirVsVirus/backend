using System.Collections.Generic;
using System.Linq;
using MySqlConnect.Data;
using MySqlConnect.Entities;

namespace ReservationSystem.Contracts
{

    public class LocationResult
    {

        public LocationResult()
        {
        }

        public LocationResult(Location location)
        {
            Id = location.Id;
            Capacity = location.Capacity;
            SlotDuration = location.SlotDuration;
            SlotSize = location.SlotSize;
            Name = location.Name;
            Openings = location.LocationOpenings.Select(x => new OpeningResult(x)).ToList();
            PlacesId = location.PlacesId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public FillStatus FillStatus { get; set; }
        public int Capacity { get; set; }
        public int SlotDuration { get; set; }
        public int SlotSize { get; set; }
        public IList<OpeningResult> Openings { get; set; }

        public string Address { get; set; }
        //Not needed in the response, but needed for LocationController.SearchRegisteredLocations
        public string PlacesId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

}
