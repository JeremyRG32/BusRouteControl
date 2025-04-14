namespace BusRouteControl.Domain.Dtos
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public string DepartureTime { get; set; } = null!;
        public string ArrivalTime { get; set; } = null!;
    }
}