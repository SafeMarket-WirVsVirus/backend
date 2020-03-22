using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySqlConnect.Entities {

  public class Reservation {

    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public string DeviceId { get; set; } // keine Tabelle notwendig

    [ForeignKey(nameof(Location))]
    public int LocationId { get; set; }

    public virtual Location Location { get; set; }

    public string ReservationToken { get; set; }

    public string HumanReadableToken { get; set; }
  }
}
