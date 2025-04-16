using BusRouteControl.Domain.Core;

namespace BusRouteControl.Domain.Entities;

public partial class User : BaseEntity
{
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
