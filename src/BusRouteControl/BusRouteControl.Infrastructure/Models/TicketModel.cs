﻿namespace BusRouteControl.Infrastructure.Models
{
    public class TicketModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ScheduleId { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = null!;
        public DateTime? BookingDate { get; set; }
    }
}
