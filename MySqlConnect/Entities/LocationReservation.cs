using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySqlConnect.Entities {

  public class LocationReservation {

    public int LocationId { get; set; }
    public int SlotSize { get; set; }
    public int SlotDuration { get; set; }
  }

}
