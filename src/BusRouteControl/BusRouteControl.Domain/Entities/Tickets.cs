using System;
using System.Collections.Generic;

namespace BusRouteControl.Web.Data.Entities;

public partial class Tickets
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ScheduleId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? BookingDate { get; set; }

    public virtual Schedules Schedule { get; set; } = null!;

    public virtual Users User { get; set; } = null!;
}
