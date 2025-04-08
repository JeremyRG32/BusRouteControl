namespace BusRouteControl.Web.Models
{
    public class TicketViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = null!;
        public DateTime? BookingDate { get; set; }
    }


}
