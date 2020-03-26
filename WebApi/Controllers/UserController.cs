using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnect.Data;
using MySqlConnect.Entities;
using ReservationSystem.Contracts;
using ReservationSystem.Extensions;


namespace ReservationSystem.Controllers {

  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase {

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserModel model) {
      using (var context = new ReservationContext()) {
        if (await context.User.AnyAsync(x => x.Email == model.Email)) {
          return Conflict();
        }

        var dbModel = new User() {
          Email = model.Email,
          Name = model.Name,
          Password = model.Password.HashAsSha256(),
          Type = model.Type
        };
        await context.User.AddAsync(dbModel);
        await context.SaveChangesAsync();

        return Ok(new UserModelWithId() {
          Id = dbModel.Id,
          Name = dbModel.Name,
          Email = dbModel.Email,
          Password = String.Empty,
          Type = dbModel.Type
        });
      }
    }

    /// <summary>
    ///   
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserModelWithId model) {
      using (var context = new ReservationContext()) {
        var user = await context.User.SingleOrDefaultAsync(x => x.Id == model.Id);

        if (user == null) {
          return NotFound();
        }

        if (await context.User.AnyAsync(x => x.Id != user.Id && x.Email == user.Email)) {
          return BadRequest("Email already used.");
        }

        user.Name = model.Name;
        user.Email = model.Email;
        user.Type = model.Type;

        if (string.IsNullOrWhiteSpace(model.Password) == false) {
          user.Password = model.Password.HashAsSha256();
        }

        model.Password = string.Empty;
        await context.SaveChangesAsync();
      }

      return Ok(model);
    }

    [HttpDelete]
    public async Task Delete(int id) {
      using (var context = new ReservationContext()) {
        var user = await context.User.SingleOrDefaultAsync(x => x.Id == id);

        if (user == null) {
          NotFound();
          return;
        }

        context.User.Remove(user);
        await context.SaveChangesAsync();

        Ok();
      }
    }

  }

}
