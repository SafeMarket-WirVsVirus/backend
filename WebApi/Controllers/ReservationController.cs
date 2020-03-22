using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnect.Data;
using MySqlConnect.Entities;
using ReservationSystem.Contracts;

namespace ReservationSystem.Controllers {

  [Route("api/[controller]")]
  [ApiController]
  public class ReservationController : ControllerBase {

      private readonly HumanReadableKeyGeneratorService _humanReadableKeyGeneratorService;

      public ReservationController(HumanReadableKeyGeneratorService humanReadableKeyGeneratorService)
      {
          _humanReadableKeyGeneratorService = humanReadableKeyGeneratorService;
      }


    [HttpPost("Reserve")]
    public async Task<IActionResult> Reserve(int locationId, DateTime dateTime, string deviceId) {
      using (var context = new ReservationContext()) {
        var location = await context.Location.FirstOrDefaultAsync(x => x.Id == locationId);

        if (location == null) return NotFound(nameof(deviceId));

        if (await IsReservationAllowedAsync(context, locationId, dateTime) == false) {
          return BadRequest("Error: Reservation not allowed.");
        }


        var humanReadableToken = _humanReadableKeyGeneratorService.GetHumanReadableText();
        var reservationToken = Guid.NewGuid().ToString();

        (location.Reservations ??= new List<Reservation>()).Add(new Reservation {
          DeviceId = deviceId,
          StartTime = dateTime,
          HumanReadableToken = humanReadableToken,
          ReservationToken = reservationToken
        });

        await context.SaveChangesAsync();

        return Ok(new CreateReservationResult()
        {
            HumanReadableToken = humanReadableToken,
            ReservationToken = reservationToken
        });
      }
    }

    private async Task<bool> IsReservationAllowedAsync(ReservationContext context, int locationId, DateTime dateTime) {
      var openingHours = await context.LocationOpening
                                      .Where(x => x.LocationId == locationId &&
                                                  x.OpeningDays == dateTime.DayOfWeek.ToString() &&
                                                  x.OpeningTime.TimeOfDay <= dateTime.TimeOfDay &&
                                                  x.ClosingTime.TimeOfDay >= dateTime.TimeOfDay)
                                      .SingleOrDefaultAsync();

      if (openingHours == null) {
        return false;
      }

      var location = await context.Location.SingleAsync(x => x.Id == locationId);
      var rangeStart = dateTime.AddMinutes(-location.SlotDuration);
      var rangeEnd = dateTime.AddMinutes(location.SlotDuration);

      var numberOfReservations = await context.Reservation.Where(x => x.LocationId == locationId &&
                                                                      ((x.StartTime >= rangeStart &&
                                                                        x.StartTime <= rangeEnd)
                                                                    || (x.StartTime.AddMinutes(location.SlotDuration) >=
                                                                        rangeStart &&
                                                                        x.StartTime.AddMinutes(location.SlotDuration) <=
                                                                        rangeEnd))).CountAsync();

      return numberOfReservations < location.SlotSize;
    }

    [HttpGet("ReservationByDevice")]
    public async Task<GetReservationsResult> GetReservations(int locationId, string deviceId) {
      using (var context = new ReservationContext()) {
        var location = await context.Location.Include(x => x.Reservations)
                                    .FirstOrDefaultAsync(x => x.Id == locationId);

        if (location == null) {
          NotFound(nameof(locationId));
          return null;
        }

        Ok();
        return new GetReservationsResult {
          Reservations = location.Reservations
                                 .Where(x => x.DeviceId == deviceId)
                                 .Select(x => new ReservationContract(x))
                                 .ToList()
        };
      }
    }


    /// <summary>
    /// Gets all reservations created by a specific device
    /// </summary>
    /// <param name="deviceId">device id</param>
    /// <returns>all reservations</returns>
    [HttpGet("ReservationsByDevice")]
    public async Task<IActionResult> ReservationsByDevice(string deviceId) // ToDo Index auf DeviceId
    {
        using (var context = new ReservationContext())
        {
            return Ok(
                new GetReservationsResult()
                {
                    Reservations = context.Reservation.Where(x => x.DeviceId == deviceId)
                        .Select(x => new ReservationContract(x))
                        .ToList()
                }
            );
        }
    }

    [HttpDelete("RevokeReservationsAtLocation")]
    public async Task<IActionResult> RevokeReservation(int locationId, string deviceId) {
      using (var context = new ReservationContext()) {
        var location = await context.Location.FirstOrDefaultAsync(x => x.Id == locationId);

        if (location == null) {
          return NotFound();
        }

        foreach (var reservation in location.Reservations.Where(x => x.DeviceId == deviceId)) {
          location.Reservations.Remove(reservation);
        }

        await context.SaveChangesAsync();

        return Ok();
      }
    }

    [HttpDelete("ReservationByLocation")]
    public async Task<List<ReservationContract>> ReservationByLocation(int locationId, DateTime day) {
      using (var context = new ReservationContext()) {
        var reservations = await context.Reservation.Where(x => x.Id == locationId
                                                             && x.StartTime >= new DateTime(day.Year, day.Month, day.Day)
                                                             && x.StartTime <= new DateTime(day.Year, day.Month, day.Day, 23, 59, 59))
                                        .Select(x => new ReservationContract(x))
                                        .ToListAsync();

        Ok();
        return reservations;
      }
    }

    /// <summary>
    /// Revokes a specific reservation at a specific location from one device
    /// </summary>
    /// <param name="locationId">lcoation id</param>
    /// <param name="deviceId">device id</param>
    /// <param name="reservationId">reservation id</param>
    /// <returns></returns>
    [HttpDelete("RevokeSpecificReservation")]
    public async Task<IActionResult> RevokeSpecificReservation(int locationId, string deviceId, int reservationId) {
      using (var context = new ReservationContext()) {
        var location = await context.Location.FirstOrDefaultAsync(x => x.Id == locationId);

        if (location == null) {
          return NotFound();
        }

        foreach (var reservation in location.Reservations.Where(x => x.Id == reservationId && x.DeviceId == deviceId)) {
          location.Reservations.Remove(reservation);
        }

        await context.SaveChangesAsync();

        return Ok();
      }
    }


    [HttpGet("ValidKeys")]
    public async Task<GetValidKeysResult> ValidKeys(int locationId, int amountTimeSlots) {
      using (var context = new ReservationContext()) {
        var location = await context.Location
              .Include(x => x.Reservations)
              .FirstOrDefaultAsync(x => x.Id == locationId);

        if (location == null) {
          NotFound(nameof(locationId));
          return null;
        }

        List<string> validKeys = context.Reservation
              .Where(x => x.LocationId == locationId
                       && x.StartTime.AddMinutes(location.SlotDuration) > DateTime.Now
                       && x.StartTime < DateTime.Now.AddMinutes(location.SlotDuration * amountTimeSlots))
              .Select(res => res.HumanReadableToken)
              .ToList();

        Ok();
        return new GetValidKeysResult {
          ValidKeys = validKeys
        };
      }
    }
  }

  public class CreateReservationResult
  {
      public string HumanReadableToken { get; set; }
      public string ReservationToken { get; set; }
  }
}
