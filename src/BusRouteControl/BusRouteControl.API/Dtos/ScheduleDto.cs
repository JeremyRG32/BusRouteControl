namespace BusRouteControl.API.Dtos
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public string DepartureTime { get; set; } = null!;
        public string ArrivalTime { get; set; } = null!;
    }
}