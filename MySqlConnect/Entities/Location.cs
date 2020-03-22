using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MySqlConnect.Data;

namespace MySqlConnect.Entities {

  public class Location {

    public Location() {
      LocationOpenings = new HashSet<LocationOpening>();
      Reservations = new List<Reservation>();
    }
    

    public int Id { get; set; }
    public FillStatus FillStatus { get; set; }
    public string Name { get; set; }

    public int Capacity { get; set; }
    public int SlotSize { get; set; }
    public int SlotDuration { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    public virtual User User { get; set; }

    public HashSet<LocationOpening> LocationOpenings { get; set; }
    public List<Reservation> Reservations { get; set; }
    public string PlacesId { get; set; }

    }

}
