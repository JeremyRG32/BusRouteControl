namespace BusRouteControl.Web.Models
{
    public class ScheduleViewModel
    {
        public int Id { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public TimeOnly ArrivalTime { get; set; }
    }
}
