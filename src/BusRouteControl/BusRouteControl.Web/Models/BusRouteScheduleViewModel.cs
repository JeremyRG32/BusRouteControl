using BusRouteControl.Domain.Entities;

namespace BusRouteControl.Web.Models
{
    public class BusRouteScheduleViewModel
    {
        public int BusRouteId { get; set; }
        public string RouteName { get; set; }
        public string RouteOrigin { get; set; }
        public string RouteDestination { get; set; }
        public List<Schedule> Schedules { get; set; }
    }
}
