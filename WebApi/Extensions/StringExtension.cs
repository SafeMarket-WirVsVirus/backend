using System.Security.Cryptography;
using System.Text;

namespace ReservationSystem.Extensions {

  public static class StringExtension {

    public static string HashAsSha256(this string plainText) {
      byte[] hash;

      using (var algorithm = SHA256.Create()) {
        hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(plainText));
      }

      StringBuilder builder = new StringBuilder();

      foreach (var item in hash) {
        builder.Append(item.ToString("X2"));
      }

      return builder.ToString();
    }

  }

}
