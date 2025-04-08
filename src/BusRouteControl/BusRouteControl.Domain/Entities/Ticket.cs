﻿namespace BusRouteControl.Domain.Entities;

public partial class Ticket
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ScheduleId { get; set; }

    public decimal Price { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? BookingDate { get; set; } = DateTime.UtcNow;

    public virtual Schedule Schedule { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
