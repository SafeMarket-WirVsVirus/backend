using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MySqlConnect.Entities {

  public class LocationOpening {

    public int Id { get; set; }
    public DateTime OpeningTime { get; set; }
    public DateTime ClosingTime { get; set; }
    public string OpeningDays { get; set; }

    [ForeignKey(nameof(Location))]
    public int LocationId { get; set; }

    public virtual Location Location { get; set; }

  }

}
