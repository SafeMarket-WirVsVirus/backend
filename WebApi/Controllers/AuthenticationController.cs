using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySqlConnect.Data;
using ReservationSystem.Contracts;
using ReservationSystem.Extensions;

namespace ReservationSystem.Controllers {

  /// <summary>
  ///     Authentication
  /// </summary>
  [Route("api/[controller]")]
  [ApiController]
  public class AuthenticationController : ControllerBase {

    public static readonly byte[] TokenEncryptionKey =
      Convert.FromBase64String("XUJXEiR2COp4p6KC9NIE6KwCD7BTxZpVcqOMSj5lgKyX2zpcXN4dismipoQIMga6X5eQmv2vZxsC5i8TCKOaM4MAEp5uIXcDkKDpOMAMbGipe171TlCDy2oLQNSSlp6v");

    public static readonly string ValidIssuer = "wirvsvirusretail.azurewebsites.net";

    public static readonly string ValidAudience = "wirvsvirusretail.azurewebsites.net";

    /// <summary>
    ///     Request a new token
    /// </summary>
    /// <param name="credentials">credentials</param>
    /// <returns></returns>
    [HttpPost] // !! Important even if it is only a Get
    public async Task<IActionResult> CreateToken([FromBody] UserCredentials credentials) {
      using (var context = new ReservationContext()) {
        var user = await context.User.SingleOrDefaultAsync(x => x.Email == credentials.Email &&
                                                                x.Password == credentials.Password.HashAsSha256());

        if (user == null) {
          return Unauthorized("Wrong username or password");
        }

        List<Claim> GetClaims() {
          // ToDo implement correctly
          return new List<Claim> {
            new Claim(ClaimTypes.Role, user.Type.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
          };
        }

        var creds = new SigningCredentials(new SymmetricSecurityKey(TokenEncryptionKey), SecurityAlgorithms.HmacSha256);
        var nowUtc = DateTime.Now.ToUniversalTime();
        var claimList = GetClaims();

        var token = new JwtSecurityToken(ValidIssuer, // ToDo es gibt BackOffice und App als Issuer
                                         ValidAudience,
                                         expires: nowUtc.AddHours(1).ToUniversalTime(),
                                         signingCredentials: creds,
                                         claims: claimList);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new LoginResponse() {
          JWTWebToken = tokenString
        });
      }
    }

    [AllowAnonymous]
    [HttpGet("Anonymous")]
    public IActionResult Anonymouse() {
      return Ok("🚀 wheey");
    }

    [Authorize] // kann auch über Controller gesetzt werden
    [HttpGet("BackofficerOnly")]
    public IActionResult BackofficerOnly() {
      return Ok("🚀 wheey");
    }

  }

}
