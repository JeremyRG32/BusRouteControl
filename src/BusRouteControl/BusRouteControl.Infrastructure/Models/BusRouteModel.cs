namespace BusRouteControl.Infrastructure.Models
{
    public class BusRouteModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Origin { get; set; } = null!;
        public string Destination { get; set; } = null!;
        public decimal DefaultPrice { get; set; }
        public List<ScheduleModel> Schedules { get; set; } = new();
    }
}
