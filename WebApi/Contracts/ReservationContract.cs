using System;
using MySqlConnect.Entities;

namespace ReservationSystem.Contracts
{
    public class ReservationContract
    {
        public ReservationContract(Reservation reservation)
        {
            Id = reservation.Id;
            StartTime = reservation.StartTime;
            DeviceId = reservation.DeviceId;
        }

        public string DeviceId { get; set; }

        public DateTime StartTime { get; set; }

        public int Id { get; set; }
    }
}