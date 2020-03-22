namespace ReservationSystem.Contracts {

  public class LocationSearchContract {

    public GeoLocation Location { get; set; }

    public string Range { get; set; }

    public string GoogleTypeFilter { get; set; }

  }

}
