namespace BusRouteControl.Application.DTOs
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public TimeOnly ArrivalTime { get; set; }
    }

}