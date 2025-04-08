using System.ComponentModel.DataAnnotations;

namespace BusRouteControl.Web.Models
{
    public class BusRouteScheduleViewModel
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public required string Name { get; set; }
        [MaxLength(50)]
        public required string Origin { get; set; }
        [MaxLength(50)]
        public required string Destination { get; set; }
        public decimal DefaultPrice { get; set; }
        public required List<ScheduleViewModel> Schedules { get; set; }
    }

}

