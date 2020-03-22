using System.Collections;
using System.Collections.Generic;
using MySqlConnect.Data;

namespace ReservationSystem.Contracts {

  public class LocationModel {

    public LocationModel() {
      LocationOpening = new List<LocationOpeningModel>();
    }

    public string Name { get; set; }
    public FillStatus FillStatus { get; set; }
    public int Capacity { get; set; }
    public int SlotSize { get; set; }
    public int SlotDuration { get; set; }
    public ShopType ShopType { get; set; }
    public int UserId { get; set; }
    public string PlacesId { get; set; }

    public IList<LocationOpeningModel> LocationOpening { get; set; }

  }

}
