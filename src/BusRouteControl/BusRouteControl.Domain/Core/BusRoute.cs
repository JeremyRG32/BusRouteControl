using BusRouteControl.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BusRouteControl.Domain.Core;

public partial class BusRoute
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    [MaxLength(50)]
    public string Origin { get; set; } = null!;
    [MaxLength(50)]
    public string Destination { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
