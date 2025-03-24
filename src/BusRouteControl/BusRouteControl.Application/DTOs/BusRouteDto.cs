namespace BusRouteControl.Application.DTOs
{
    public class BusRouteDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Origin { get; set; }
        public string? Destination { get; set; }
        public List<ScheduleDto>? Schedules { get; set; } = new List<ScheduleDto>();
    }

}
