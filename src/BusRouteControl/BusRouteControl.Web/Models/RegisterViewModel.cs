namespace BusRouteControl.Web.Models
{
    public class RegisterViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public List<TicketViewModel> Tickets { get; set; } = new();
    }
}
