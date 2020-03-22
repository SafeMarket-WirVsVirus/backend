using System.Collections.Generic;
using MySqlConnect.Data;

namespace MySqlConnect.Entities {

  public class User {

    public User() {
      Locations = new HashSet<Location>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserType Type { get; set; }

    public HashSet<Location> Locations { get; set; }

  }

}
