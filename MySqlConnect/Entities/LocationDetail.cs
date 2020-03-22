using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySqlConnect.Entities {

  public class LocationDetail {

    public int Id { get; set; }
    //(in minutes?)
    public int SlotDuration  { get; set; }
    //(for the next three days?)
    public bool FillStatusPerSlot  { get; set; }
    public int Reservations  { get; set; }
  }

}
