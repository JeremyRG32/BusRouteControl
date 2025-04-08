using Microsoft.AspNetCore.Mvc;


namespace BusRouteControl.Web.Controllers
{
    public class TicketController : Controller
    {
        private readonly HttpClient _httpClient;

        public TicketController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
