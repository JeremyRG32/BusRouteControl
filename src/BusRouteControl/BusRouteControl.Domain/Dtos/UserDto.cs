namespace BusRouteControl.Domain.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public List<TicketDto> Tickets { get; set; } = new();
    }
}
