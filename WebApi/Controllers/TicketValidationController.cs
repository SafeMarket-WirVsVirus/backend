using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnect.Data;
using ReservationSystem.Contracts;

namespace ReservationSystem.Controllers
{
    public class TicketValidationController : ControllerBase
    {
        [HttpGet("VerifyTicket")]
        public async Task<IActionResult> Task(string ticketId)
        {
            using (var context = new ReservationContext())
            {
                var reservation = await context.Reservation
                    .Include(x => x.Location)
                    .FirstOrDefaultAsync(x => x.ReservationToken == ticketId);

                if (reservation == null)
                {
                    return Ok(new TicketValidationResult()
                    {
                        ValidationResult = false
                    });
                }
                
                var currentTime = DateTime.Now.ToUniversalTime();
                if (reservation.StartTime <= currentTime &&
                    currentTime <= reservation.StartTime.AddMinutes(reservation.Location.SlotDuration))
                {
                    return Ok(new TicketValidationResult()
                    {
                        ValidationResult = true
                    });
                }

                return Ok(new TicketValidationResult()
                {
                    ValidationResult = false
                });
            }
        }

    }
}
