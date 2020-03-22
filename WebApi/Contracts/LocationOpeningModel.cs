using System;

namespace ReservationSystem.Contracts {

  public class LocationOpeningModel {

    public string DayOfWeek { get; set; }

    public DateTime OpeningHours { get; set; }

    public DateTime ClosingHours { get; set; }

  }

}
