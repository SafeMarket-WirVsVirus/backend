using System;
using System.Collections.Generic;

namespace ReservationSystem.Contracts {

  public class SlotResult {

    public SlotResult() {
      Items = new List<SlotItem>();
    }

    public IList<SlotItem> Items { get; set; }

  }

  public class SlotItem {

    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int RegistrationCount { get; set; }

  }

}
