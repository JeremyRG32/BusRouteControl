using BusRouteControl.Domain.Dtos;
using BusRouteControl.Web.Models;
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
        public async Task<IActionResult> Index()
        {
            var role = HttpContext.Session.GetString("UserRole");
            var email = HttpContext.Session.GetString("UserEmail");

            var ticketsResponse = await _httpClient.GetAsync("https://localhost:7192/api/Ticket/GetAll");
            if (!ticketsResponse.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var ticketDtos = await ticketsResponse.Content.ReadFromJsonAsync<List<TicketDto>>();

            int clientId = 0;
            if (role == "Client")
            {
                var userResponse = await _httpClient.GetAsync($"https://localhost:7192/api/User/GetByEmail/{email}");
                if (!userResponse.IsSuccessStatusCode)
                    return Unauthorized();

                var userDto = await userResponse.Content.ReadFromJsonAsync<UserDto>();
                clientId = userDto.Id;
            }

            var ticketViewModels = new List<TicketViewModel>();

            foreach (var ticketDto in ticketDtos)
            {
                // If client, skip tickets that aren't theirs
                if (role == "Client" && ticketDto.UserId != clientId)
                    continue;

                var userResponse = await _httpClient.GetAsync($"https://localhost:7192/api/User/Get/{ticketDto.UserId}");
                var userDto = await userResponse.Content.ReadFromJsonAsync<UserDto>();

                var scheduleResponse = await _httpClient.GetAsync($"https://localhost:7192/api/Schedule/Get/{ticketDto.ScheduleId}");
                var scheduleDto = await scheduleResponse.Content.ReadFromJsonAsync<ScheduleDto>();

                var routeResponse = await _httpClient.GetAsync($"https://localhost:7192/BusRoute/Get/{scheduleDto.RouteId}");
                var routeDto = await routeResponse.Content.ReadFromJsonAsync<BusRouteDto>();

                var ticketViewModel = new TicketViewModel
                {
                    Id = ticketDto.Id,
                    UserName = userDto.Name,
                    DepartureTime = scheduleDto.DepartureTime,
                    ArrivalTime = scheduleDto.ArrivalTime,
                    Price = ticketDto.Price,
                    RouteName = routeDto.Name,
                    Status = ticketDto.Status,
                    BookingDate = ticketDto.BookingDate
                };

                ticketViewModels.Add(ticketViewModel);
            }

            ViewBag.Role = role;
            return View(ticketViewModels);
        }
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var ticketResponse = await _httpClient.GetAsync($"https://localhost:7192/api/Ticket/Get/{id}");
            if (!ticketResponse.IsSuccessStatusCode)
                return NotFound();

            var ticketDto = await ticketResponse.Content.ReadFromJsonAsync<TicketDto>();

            ticketDto.Status = "Cancelled";

            var updateResponse = await _httpClient.PutAsJsonAsync($"https://localhost:7192/api/Ticket/Update/{id}", ticketDto);
            if (!updateResponse.IsSuccessStatusCode)
                return BadRequest();

            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var Ticket = new TicketViewModel
            {

            };
            return View(Ticket);
        }
    }
}
