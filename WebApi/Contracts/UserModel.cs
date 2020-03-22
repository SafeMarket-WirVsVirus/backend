using MySqlConnect.Data;

namespace ReservationSystem.Contracts {

  public class UserModel {

    public string Name { get; set; }
    public string Email { get; set; }

    public string Password { get; set; }
    public UserType Type { get; set; }

  }

  public class UserModelWithId : UserModel {

    public int Id { get; set; }

  }

}
