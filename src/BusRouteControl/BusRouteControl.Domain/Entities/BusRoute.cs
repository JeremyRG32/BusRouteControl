namespace BusRouteControl.Domain.Entities;

public partial class BusRoute
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Origin { get; set; } = null!;

    public string Destination { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
