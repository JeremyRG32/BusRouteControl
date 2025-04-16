using BusRouteControl.Domain.Core;

namespace BusRouteControl.Domain.Entities;

public partial class Schedule : BaseEntity
{
    public int RouteId { get; set; }

    public TimeOnly DepartureTime { get; set; }

    public TimeOnly ArrivalTime { get; set; }

    public virtual BusRoute Route { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
