using System;
using System.Collections.Generic;

namespace BusRouteControl.Web.Data.Entities;

public partial class Routes
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Origin { get; set; } = null!;

    public string Destination { get; set; } = null!;

    public virtual ICollection<Schedules> Schedules { get; set; } = new List<Schedules>();
}
