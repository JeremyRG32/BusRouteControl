using System.ComponentModel.DataAnnotations;

namespace BusRouteControl.Web.Models
{
    public class BusRouteScheduleViewModel
    {
        public int BusRouteId { get; set; }
        [MaxLength(50)]
        public required string RouteName { get; set; }
        [MaxLength(50)]
        public required string RouteOrigin { get; set; }
        [MaxLength(50)]
        public required string RouteDestination { get; set; }
        public required List<ScheduleViewModel> Schedules { get; set; }
    }

}

