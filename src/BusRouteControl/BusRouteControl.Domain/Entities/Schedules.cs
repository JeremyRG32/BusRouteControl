namespace BusRouteControl.Domain.Entities;

public partial class Schedules
{
    public int Id { get; set; }

    public int RouteId { get; set; }

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public virtual Routes Route { get; set; } = null!;

    public virtual ICollection<Tickets> Tickets { get; set; } = new List<Tickets>();
}
