using System;
using MySqlConnect.Entities;

namespace ReservationSystem.Contracts {

  public class OpeningResult {

    public OpeningResult(LocationOpening locationOpening) {
      ClosingTime = locationOpening.ClosingTime;
      OpeningTime = locationOpening.OpeningTime;
      OpeningDays = locationOpening.OpeningDays;
    }

    public string OpeningDays { get; set; }
    public DateTime OpeningTime { get; set; }
    public DateTime ClosingTime { get; set; }

  }

}
